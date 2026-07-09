namespace ArgosApi.Domain.Entities
{
    public class Relatorio : BaseEntity
    {
        public string Nome { get; set; } = "";
        public required long ProjetoId { get; set; }
        public required Projeto Projeto { get; set; }
        public DateTime DataHoraExecucao { get; set; }
        public int Pontuacao { get; set; }
        public string Json { get; set; } = "";
        public bool TradutorLibrasIdentificado { get; set; }
        public int QuantidadeErros { get; set; }
        public int QuantidadeAvisos { get; set; }

    }
}