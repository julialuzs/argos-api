using System.Text.Json;
using ArgosApi.Data;
using ArgosApi.Domain.Entities;
using ArgosApi.Domain.Enums;
using ArgosApi.Features.Relatorios.Auditoria;
using ArgosApi.Features.Relatorios.Helpers;
using ArgosApi.Features.Relatorios.Requests;
using ArgosApi.Features.Relatorios.Responses;
using ArgosApi.Features.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Resultado da tentativa de iniciar uma auditoria sob demanda
    /// </summary>
    public record IniciarAuditoriaResultado(int StatusCode, string? Message);

    /// <summary>
    /// Service responsável por gerenciar os relatórios
    /// </summary>
    public class RelatoriosService(AppDbContext context, CurrentUser currentUser, AuditoriaQueue auditoriaQueue)
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        /// <summary>
        /// Busca relatório pelo Guid do projeto e o id do relatório
        /// </summary>
        public async Task<RelatorioDetalheResponse?> GetRelatorioPorId(
            Guid guidProjeto, long idRelatorio, CancellationToken cancellationToken)
        {
            var relatorio = await context.Relatorios
                .AsNoTracking()
                .Where(r => r.Id == idRelatorio
                    && r.Projeto.Guid == guidProjeto
                    && r.Projeto.Usuarios.Any(u => u.Id == currentUser.Id))
                .FirstOrDefaultAsync(cancellationToken);
            if (relatorio is null)
            {
                return null;
            }

            return RelatorioAuditoriaMapper.MapearParaDetalhe(relatorio);
        }

        /// <summary>
        /// Busca todos os relatórios pelo Guid público do projeto
        /// </summary>
        public async Task<IEnumerable<Relatorio>?> ListarRelatoriosPorProjeto(
            Guid guidProjeto, CancellationToken cancellationToken)
        {
            var projetoId = await ObterIdProjetoDoUsuario(guidProjeto, cancellationToken);
            if (projetoId is null)
            {
                return null;
            }

            return await context.Relatorios
                .AsNoTracking()
                .Where(relatorio => relatorio.ProjetoId == projetoId)
                .OrderByDescending(r => r.DataHoraExecucao)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Salvar relatorio na base de dados
        /// </summary>
        public async Task SalvarRelatorio(RelatorioRequest request, CancellationToken cancellationToken)
        {
            var jsonText = request.Json.GetRawText();
            RelatorioAuditoriaJson auditoria;

            try
            {
                auditoria = JsonSerializer.Deserialize<RelatorioAuditoriaJson>(jsonText, JsonOptions)
                    ?? throw new JsonException("JSON do relatório é nulo.");
            }
            catch (JsonException ex)
            {
                throw new RelatorioJsonInvalidoException(
                    $"O JSON do relatório não pôde ser processado. Path: {ex.Path ?? "(desconhecido)"}. Detalhe: {ex.Message}",
                    ex);
            }

            if (request.GuidProjeto == Guid.Empty)
            {
                throw new KeyNotFoundException("Projeto não encontrado.");
            }

            var projeto = await context.Projetos.FirstOrDefaultAsync(p => p.Guid == request.GuidProjeto, cancellationToken);
            if (projeto is null)
            {
                throw new KeyNotFoundException("Projeto não encontrado.");
            }
            var relatorio = new Relatorio
            {
                Json = jsonText,
                ProjetoId = projeto.Id,
                DataHoraExecucao = auditoria?.AuditDate ?? DateTime.UtcNow,
                Pontuacao = auditoria?.Summary?.Score ?? 0,
                TradutorLibrasIdentificado = auditoria?.Summary.AssistiveTechnologies?.VLibras ?? auditoria?.Summary.AssistiveTechnologies?.HandTalk ?? false,
                QuantidadeErros = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, SeveridadeEnum.Serious, SeveridadeEnum.Critical),
                QuantidadeAvisos = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, SeveridadeEnum.Moderate, SeveridadeEnum.Minor)
            };

            projeto.UltimaExecucao = relatorio.DataHoraExecucao;
            projeto.StatusExecucao = StatusExecucao.Idle;
            projeto.MensagemErroExecucao = null;

            await context.Relatorios.AddAsync(relatorio, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Enfileira a execução sob demanda do avaliador para o projeto
        /// </summary>
        public async Task<IniciarAuditoriaResultado> IniciarAuditoria(Guid guidProjeto, CancellationToken cancellationToken)
        {
            var projeto = await context.Projetos
                .AsNoTracking()
                .Where(p => p.Guid == guidProjeto && p.Usuarios.Any(u => u.Id == currentUser.Id))
                .FirstOrDefaultAsync(cancellationToken);

            if (projeto is null)
            {
                return new IniciarAuditoriaResultado(StatusCodes.Status404NotFound, null);
            }

            if (string.IsNullOrWhiteSpace(projeto.UrlBase))
            {
                return new IniciarAuditoriaResultado(
                    StatusCodes.Status400BadRequest,
                    "Informe a URL base do projeto antes de executar a análise.");
            }

            if (projeto.StatusExecucao == StatusExecucao.Executando)
            {
                return new IniciarAuditoriaResultado(
                    StatusCodes.Status409Conflict,
                    "Já existe uma análise em andamento para este projeto.");
            }

            var atualizados = await context.Projetos
                .Where(p => p.Id == projeto.Id && p.StatusExecucao != StatusExecucao.Executando)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(p => p.StatusExecucao, StatusExecucao.Executando)
                        .SetProperty(p => p.MensagemErroExecucao, (string?)null),
                    cancellationToken);

            if (atualizados == 0)
            {
                return new IniciarAuditoriaResultado(
                    StatusCodes.Status409Conflict,
                    "Já existe uma análise em andamento para este projeto.");
            }

            await auditoriaQueue.EnqueueAsync(projeto.Id, cancellationToken);
            return new IniciarAuditoriaResultado(StatusCodes.Status202Accepted, null);
        }

        private async Task<long?> ObterIdProjetoDoUsuario(Guid guidProjeto, CancellationToken cancellationToken)
        {
            return await context.Projetos
                .AsNoTracking()
                .Where(p => p.Guid == guidProjeto && p.Usuarios.Any(u => u.Id == currentUser.Id))
                .Select(p => (long?)p.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}