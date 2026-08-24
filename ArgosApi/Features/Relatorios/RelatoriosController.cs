using System.Text.Json;
using ArgosApi.Domain.Entities;
using ArgosApi.Features.Relatorios.Requests;
using ArgosApi.Features.Relatorios.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Controller responsável por gerenciar os projetos
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RelatoriosController(
        RelatoriosService relatoriosService
    ) : ControllerBase
    {
        /// <summary>
        /// Busca o relatorio pelo id informado
        /// </summary>
        /// <param name="idProjeto"></param>
        /// <param name="idRelatorio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Projeto</returns>
        [HttpGet("{idProjeto}/{idRelatorio}")]
        [Authorize]
        public async Task<ActionResult<RelatorioDetalheResponse>> GetPorId(
            [FromRoute] long idProjeto, [FromRoute] long idRelatorio, CancellationToken cancellationToken = default)
        {
            var response = await relatoriosService.GetRelatorioPorId(idRelatorio, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Busca os relatorios pelo id do projeto informado
        /// </summary>
        /// <param name="idProjeto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Projetos</returns>
        [HttpGet("{idProjeto}/listar")]
        [Authorize]
        public async Task<ActionResult<List<Relatorio>>> ListarRelatoriosPorProjeto(
            [FromRoute] long idProjeto, CancellationToken cancellationToken = default)
        {
            var response = await relatoriosService.ListarRelatoriosPorProjeto(idProjeto, cancellationToken);
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
        [HttpPost("{idProjeto}/executar")]
        [Authorize]
        public async Task<ActionResult> Executar(
            [FromRoute] long idProjeto, CancellationToken cancellationToken = default)
        {
            var resultado = await relatoriosService.IniciarAuditoria(idProjeto, cancellationToken);
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