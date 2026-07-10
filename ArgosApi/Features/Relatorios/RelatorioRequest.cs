using System.Text.Json;

namespace ArgosApi.Features.Relatorios
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
        /// Id do projeto relacionado
        /// </summary>
        public long IdProjeto { get; set; }
    } 
}