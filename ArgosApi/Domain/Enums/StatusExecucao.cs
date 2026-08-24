using System.Text.Json.Serialization;

namespace ArgosApi.Domain.Enums
{
    /// <summary>
    /// Status da execução sob demanda do avaliador
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusExecucao
    {
        Idle = 0,
        Executando = 1,
        Falhou = 2
    }
}
