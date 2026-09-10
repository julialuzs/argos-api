using System.Text.Json.Serialization;
using ArgosApi.Domain.Enums;

namespace ArgosApi.Domain.Entities
{
    /// <summary>
    /// Projeto auditado pela plataforma
    /// </summary>
    public class Projeto : BaseEntity
    {
        /// <summary>
        /// Nome do projeto
        /// </summary>
        public string Nome { get; set; } = "";

        /// <summary>
        /// Descrição do projeto
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Identificador público do projeto
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Data/hora da última execução do avaliador
        /// </summary>
        public DateTime? UltimaExecucao { get; set; }

        /// <summary>
        /// URL base do site auditado
        /// </summary>
        public string UrlBase { get; set; } = "";

        /// <summary>
        /// Rotas a serem auditadas
        /// </summary>
        public string[] Rotas { get; set; } = ["/"];

        /// <summary>
        /// Indica se a auditoria deve incluir regras W3C
        /// </summary>
        public bool IncluirW3c { get; set; }

        /// <summary>
        /// Status da última execução do avaliador
        /// </summary>
        public StatusExecucao StatusExecucao { get; set; } = StatusExecucao.Idle;

        /// <summary>
        /// Mensagem de erro da última execução, quando houver
        /// </summary>
        public string? MensagemErroExecucao { get; set; }

        /// <summary>
        /// Usuários vinculados ao projeto
        /// </summary>
        [JsonIgnore]
        public ICollection<Usuario> Usuarios { get; set; } = [];

        /// <summary>
        /// Relatórios gerados para o projeto
        /// </summary>
        [JsonIgnore]
        public ICollection<Relatorio> Relatorios { get; set; } = [];
    }
}
