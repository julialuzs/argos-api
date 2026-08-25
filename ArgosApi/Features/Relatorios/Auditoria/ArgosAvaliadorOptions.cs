namespace ArgosApi.Features.Relatorios.Auditoria
{
    /// <summary>
    /// Opções de configuração do avaliador de acessibilidade
    /// </summary>
    public class ArgosAvaliadorOptions
    {
        /// <summary>
        /// Nome da seção no arquivo de configuração
        /// </summary>
        public const string SectionName = "ArgosAvaliador";

        /// <summary>
        /// Caminho do executável Node.js
        /// </summary>
        public string NodePath { get; set; } = "node";

        /// <summary>
        /// Caminho do ponto de entrada da CLI do avaliador
        /// </summary>
        public string CliEntryPoint { get; set; } = "";

        /// <summary>
        /// Diretório de trabalho da CLI do avaliador
        /// </summary>
        public string WorkingDirectory { get; set; } = "";

        /// <summary>
        /// Tempo máximo de execução da auditoria, em minutos
        /// </summary>
        public int TimeoutMinutes { get; set; } = 10;

        /// <summary>
        /// Endpoint da API para envio dos relatórios gerados
        /// </summary>
        public string ApiRelatoriosEndpoint { get; set; } = "https://localhost:7202/relatorios";
    }
}
