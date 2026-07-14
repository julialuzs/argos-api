namespace ArgosApi.Domain.Entities
{
    /// <summary>
    /// Relatório
    /// </summary>
    public class Relatorio : BaseEntity
    {
        /// <summary>
        /// Id do projeto
        /// </summary>
        public required long ProjetoId { get; set; }
        /// <summary>
        /// Referência ao projeto
        /// </summary>
        public Projeto Projeto { get; set; }
        /// <summary>
        /// Data/hora em que o relatório foi executado
        /// </summary>
        public DateTime DataHoraExecucao { get; set; }
        /// <summary>
        /// Pontuação definida pelo avaliador de acessibilidade
        /// </summary>
        public int Pontuacao { get; set; }
        /// <summary>
        /// JSON retornado pelo avaliador de acessibiliade contendo os apontamentos
        /// </summary>
        public string Json { get; set; } = "";
        /// <summary>
        /// Indica se foi identificado algum tradutor de Libras como VLibras
        /// </summary>
        public bool TradutorLibrasIdentificado { get; set; }
        /// <summary>
        /// Quantidade de erros identificados
        /// </summary>
        public int QuantidadeErros { get; set; }
        /// <summary>
        /// Quantidade de avisos identificados
        /// </summary>
        public int QuantidadeAvisos { get; set; }

    }
}