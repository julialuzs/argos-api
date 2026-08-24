using System.Text.Json.Serialization;
using ArgosApi.Domain.Enums;

namespace ArgosApi.Domain.Entities
{
    public class Projeto : BaseEntity
    {
        public string Nome { get; set; } = "";

        public string Descricao { get; set; } = "";

        public DateTime? UltimaExecucao { get; set; }

        public string UrlBase { get; set; } = "";

        public string[] Rotas { get; set; } = ["/"];

        public bool IncluirW3c { get; set; }

        public StatusExecucao StatusExecucao { get; set; } = StatusExecucao.Idle;

        public string? MensagemErroExecucao { get; set; }

        [JsonIgnore]
        public ICollection<Usuario> Usuarios { get; set; } = [];

        [JsonIgnore]
        public ICollection<Relatorio> Relatorios { get; set; } = [];
    }
}
