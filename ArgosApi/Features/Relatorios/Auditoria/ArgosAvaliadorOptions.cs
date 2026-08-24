namespace ArgosApi.Features.Relatorios.Auditoria
{
    public class ArgosAvaliadorOptions
    {
        public const string SectionName = "ArgosAvaliador";

        public string NodePath { get; set; } = "node";

        public string CliEntryPoint { get; set; } = "";

        public string WorkingDirectory { get; set; } = "";

        public int TimeoutMinutes { get; set; } = 10;

        public string ApiRelatoriosEndpoint { get; set; } = "https://localhost:7202/relatorios";
    }
}
