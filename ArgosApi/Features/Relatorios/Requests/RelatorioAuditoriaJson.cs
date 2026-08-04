namespace ArgosApi.Features.Relatorios.Requests
{
    /// <summary>
    /// Estrutura do JSON retornado pelo avaliador de acessibilidade
    /// </summary>
    public class RelatorioAuditoriaJson
    {
        /// <summary>
        /// Resumo da auditoria
        /// </summary>
        public SummaryJson Summary { get; set; }
        
        // TODO: melhorar json para que ele não tenha nomes em ingles
        /// <summary>
        /// Resultados por rota auditada
        /// </summary>
        public List<ResultadoAuditoriaJson> Results { get; set; } = [];

        /// <summary>
        /// Pontuação
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// Duração da execução da auditoria em milissegundos
        /// </summary>
        public long DurationMs { get; set; }

        /// <summary>
        /// Data/hora em que a auditoria foi executada
        /// </summary>
        public DateTime? AuditDate { get; set; }
    }

    public class SummaryJson
    {
        public int Score { get; set; }

        public Dictionary<SeveridadeEnum, int> BySeverity { get; set; }

        /// <summary>
        /// Quantidade de fluxos auditados
        /// </summary>
        public int FlowsAudited { get; set; }

        /// <summary>
        /// Quantidade de rotas auditadas
        /// </summary>
        public int RoutesAudited { get; set; }

        /// <summary>
        /// Quantidade de apontamentos
        /// </summary>
        public int TotalFindings { get; set; }

        /// <summary>
        /// Tecnologias assistivas identificadas no site
        /// </summary>
        public TecnologiasAssistivasJson? AssistiveTechnologies { get; set; }
    }

    /// <summary>
    /// Resultado da auditoria de uma rota
    /// </summary>
    public class ResultadoAuditoriaJson 
    {
        /// <summary>
        /// URL auditada
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Caminho da rota auditada
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Pontuação da rota
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Apontamentos encontrados na rota
        /// </summary>
        public List<ApontamentoJson> Findings { get; set; } = [];

        /// <summary>
        /// Critérios eMAG mapeados na rota
        /// </summary>
        public List<string> EmagMappings { get; set; } = [];

        /// <summary>
        /// Quantidade de problemas críticos na rota
        /// </summary>
        public int CriticalIssues { get; set; }
    }

    /// <summary>
    /// Apontamento de acessibilidade
    /// </summary>
    public class ApontamentoJson
    {
        /// <summary>
        /// Identificador do apontamento
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Título do apontamento
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Severidade do apontamento.
        /// </summary>
        public SeveridadeEnum Severity { get; set; }

        /// <summary>
        /// Ferramenta que originou o apontamento
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// Descrição do apontamento
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Critérios eMAG relacionados
        /// </summary>
        public List<string> EmagCriteria { get; set; } = [];

        /// <summary>
        /// Recomendação de correção
        /// </summary>
        public string? Recommendation { get; set; }

        /// <summary>
        /// URL de ajuda sobre o apontamento
        /// </summary>
        public string? HelpUrl { get; set; }

        /// <summary>
        /// Referências WCAG relacionadas
        /// </summary>
        public List<string> WcagRefs { get; set; } = [];

        /// <summary>
        /// Elemento HTML relacionado ao apontamento
        /// </summary>
        public string? HtmlElement { get; set; }

        /// <summary>
        /// Quantidade de elementos afetados
        /// </summary>
        public int? ElementCount { get; set; }
    }

    /// <summary>
    /// Tecnologias assistivas detectadas
    /// </summary>
    public class TecnologiasAssistivasJson
    {
        /// <summary>
        /// Indica presença do VLibras
        /// </summary>
        public bool VLibras { get; set; }

        /// <summary>
        /// Indica presença do Hand Talk
        /// </summary>
        public bool HandTalk { get; set; }
    }
}
