using System.Text.Json;
using ArgosApi.Domain.Entities;
using ArgosApi.Features.Relatorios.Requests;
using ArgosApi.Features.Relatorios.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Controller responsável por gerenciar os relatórios
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RelatoriosController(
        RelatoriosService relatoriosService
    ) : ControllerBase
    {
        /// <summary>
        /// Busca o relatório pelo Guid do projeto e o id do relatório
        /// </summary>
        /// <param name="guidProjeto">Identificador público do projeto</param>
        /// <param name="idRelatorio"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{guidProjeto:guid}/{idRelatorio:long}")]
        [Authorize]
        public async Task<ActionResult<RelatorioDetalheResponse>> GetPorId(
            [FromRoute] Guid guidProjeto, [FromRoute] long idRelatorio, CancellationToken cancellationToken = default)
        {
            var response = await relatoriosService.GetRelatorioPorId(guidProjeto, idRelatorio, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Busca os relatórios pelo Guid do projeto informado
        /// </summary>
        /// <param name="guidProjeto">Identificador público do projeto</param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{guidProjeto:guid}/listar")]
        [Authorize]
        public async Task<ActionResult<List<Relatorio>>> ListarRelatoriosPorProjeto(
            [FromRoute] Guid guidProjeto, CancellationToken cancellationToken = default)
        {
            var response = await relatoriosService.ListarRelatoriosPorProjeto(guidProjeto, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Salva o relatorio em JSON
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SalvarRelatorio([FromBody] RelatorioRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                await relatoriosService.SalvarRelatorio(request, cancellationToken);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Projeto não encontrado." });
            }
            catch (RelatorioJsonInvalidoException ex)
            {
                var jsonEx = ex.InnerException as JsonException;
                return UnprocessableEntity(new
                {
                    message = ex.Message,
                    detail = jsonEx?.Message,
                    path = jsonEx?.Path,
                    line = jsonEx?.LineNumber,
                    position = jsonEx?.BytePositionInLine
                });
            }
        }

        /// <summary>
        /// Dispara a execução sob demanda do avaliador Argos para o projeto
        /// </summary>
        [HttpPost("{guidProjeto:guid}/executar")]
        [Authorize]
        public async Task<ActionResult> Executar(
            [FromRoute] Guid guidProjeto, CancellationToken cancellationToken = default)
        {
            var resultado = await relatoriosService.IniciarAuditoria(guidProjeto, cancellationToken);
            return resultado.StatusCode switch
            {
                StatusCodes.Status202Accepted => Accepted(),
                StatusCodes.Status400BadRequest => BadRequest(new { message = resultado.Message }),
                StatusCodes.Status409Conflict => Conflict(new { message = resultado.Message }),
                _ => NotFound()
            };
        }
    }
}
