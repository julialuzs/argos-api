using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Dashboard
{
    /// <summary>
    /// Controller responsável pelos dados da tela de dashboard
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("projetos/{idProjeto:long}/dashboard")]
    public class DashboardController(DashboardService dashboardService) : ControllerBase
    {
        /// <summary>
        /// Busca os dados agregados do dashboard do projeto
        /// </summary>
        /// <param name="idProjeto">Id do projeto</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        public async Task<ActionResult<DashboardResponse>> Get(
            [FromRoute] long idProjeto,
            CancellationToken cancellationToken = default)
        {
            var response = await dashboardService.GetDashboard(idProjeto, cancellationToken);
            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
