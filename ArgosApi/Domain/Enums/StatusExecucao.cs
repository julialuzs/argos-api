using System.Text.Json.Serialization;

namespace ArgosApi.Domain.Enums
{
    /// <summary>
    /// Status da execução sob demanda do avaliador
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusExecucao
    {
        /// <summary>
        /// Nenhuma execução em andamento
        /// </summary>
        Idle = 0,

        /// <summary>
        /// Avaliação em andamento
        /// </summary>
        Executando = 1,

        /// <summary>
        /// Última avaliação falhou
        /// </summary>
        Falhou = 2
    }
}
