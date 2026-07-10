namespace ArgosApi.Domain.Entities
{
    /// <summary>
    /// Relatório
    /// </summary>
    public class Relatorio : BaseEntity
    {
        public required long ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
        public DateTime DataHoraExecucao { get; set; }
        public int Pontuacao { get; set; }
        public string Json { get; set; } = "";
        public bool TradutorLibrasIdentificado { get; set; }
        public int QuantidadeErros { get; set; }
        public int QuantidadeAvisos { get; set; }

    }
}