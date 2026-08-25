using System.Text.Json;

namespace ArgosApi.Features.Relatorios.Requests
{
    /// <summary>
    /// Request para criação de relatório
    /// </summary>
    public class RelatorioRequest
    {
        /// <summary>
        /// JSON contendo relatório
        /// </summary>
        public JsonElement Json { get; set; }
        
        /// <summary>
        /// Guid público do projeto relacionado
        /// </summary>
        public Guid GuidProjeto { get; set; }
    }
}