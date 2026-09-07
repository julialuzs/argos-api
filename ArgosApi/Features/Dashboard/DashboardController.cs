using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Dashboard
{
    /// <summary>
    /// Controller responsável pelos dados da tela de dashboard
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("projetos/{guidProjeto:guid}/dashboard")]
    public class DashboardController(DashboardService dashboardService) : ControllerBase
    {
        /// <summary>
        /// Busca os dados agregados do dashboard do projeto
        /// </summary>
        /// <param name="guidProjeto">Identificador público do projeto</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        public async Task<ActionResult<DashboardResponse>> Get(
            [FromRoute] Guid guidProjeto,
            CancellationToken cancellationToken = default)
        {
            var response = await dashboardService.GetDashboard(guidProjeto, cancellationToken);
            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
