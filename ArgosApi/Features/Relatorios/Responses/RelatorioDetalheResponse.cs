namespace ArgosApi.Features.Relatorios.Responses
{
    /// <summary>
    /// Resposta detalhada de um relatório com apontamentos já tratados
    /// </summary>
    public class RelatorioDetalheResponse
    {
        /// <summary>
        /// Id do relatório
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Id do projeto
        /// </summary>
        public long ProjetoId { get; set; }

        /// <summary>
        /// Data/hora em que o relatório foi executado
        /// </summary>
        public DateTime DataHoraExecucao { get; set; }

        /// <summary>
        /// Pontuação média da auditoria
        /// </summary>
        public int Pontuacao { get; set; }

        /// <summary>
        /// Indica se o VLibras foi identificado
        /// </summary>
        public bool TradutorLibrasIdentificado { get; set; }

        /// <summary>
        /// Indica se o Hand Talk foi identificado
        /// </summary>
        public bool HandTalkIdentificado { get; set; }

        /// <summary>
        /// Quantidade de erros identificados
        /// </summary>
        public int QuantidadeErros { get; set; }

        /// <summary>
        /// Quantidade de avisos identificados
        /// </summary>
        public int QuantidadeAvisos { get; set; }

        /// <summary>
        /// Quantidade de rotas auditadas
        /// </summary>
        public int RotasAuditadas { get; set; }

        /// <summary>
        /// Quantidade de fluxos auditados
        /// </summary>
        public int FluxosAuditados { get; set; }

        /// <summary>
        /// Resultados por rota auditada
        /// </summary>
        public List<ResultadoAuditoriaResponse> Resultados { get; set; } = [];
    }

    /// <summary>
    /// Resultado da auditoria de uma rota
    /// </summary>
    public class ResultadoAuditoriaResponse
    {
        /// <summary>
        /// URL auditada
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// Caminho da rota auditada
        /// </summary>
        public string Caminho { get; set; } = "";

        /// <summary>
        /// Pontuação da rota
        /// </summary>
        public int Pontuacao { get; set; }

        /// <summary>
        /// Quantidade de problemas críticos na rota
        /// </summary>
        public int ProblemasCriticos { get; set; }

        /// <summary>
        /// Critérios eMAG mapeados na rota
        /// </summary>
        public List<string> CriteriosEmagMapeados { get; set; } = [];

        /// <summary>
        /// Apontamentos encontrados na rota
        /// </summary>
        public List<ApontamentoResponse> Apontamentos { get; set; } = [];
    }

    /// <summary>
    /// Apontamento de acessibilidade tratado
    /// </summary>
    public class ApontamentoResponse
    {
        /// <summary>
        /// Identificador do apontamento
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Título do apontamento
        /// </summary>
        public string Titulo { get; set; } = "";

        /// <summary>
        /// Severidade do apontamento
        /// </summary>
        public string Severidade { get; set; } = "";

        /// <summary>
        /// Tipo do apontamento: erro ou aviso
        /// </summary>
        public string Tipo { get; set; } = "";

        /// <summary>
        /// Ferramenta que originou o apontamento
        /// </summary>
        public string Fonte { get; set; } = "";

        /// <summary>
        /// Descrição do apontamento
        /// </summary>
        public string Descricao { get; set; } = "";

        /// <summary>
        /// Critérios eMAG relacionados
        /// </summary>
        public List<string> CriteriosEmag { get; set; } = [];

        /// <summary>
        /// Recomendação de correção
        /// </summary>
        public string Recomendacao { get; set; } = "";

        /// <summary>
        /// URL de ajuda sobre o apontamento
        /// </summary>
        public string? UrlAjuda { get; set; }

        /// <summary>
        /// Referências WCAG relacionadas
        /// </summary>
        public List<string> ReferenciasWcag { get; set; } = [];

        /// <summary>
        /// Elemento HTML relacionado ao apontamento
        /// </summary>
        public string? ElementoHtml { get; set; }

        /// <summary>
        /// Quantidade de elementos afetados
        /// </summary>
        public int? QuantidadeElementos { get; set; }
    }
}
