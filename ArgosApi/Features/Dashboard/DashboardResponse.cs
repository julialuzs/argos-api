namespace ArgosApi.Features.Dashboard
{
    /// <summary>
    /// Dados agregados para os gráficos da tela de dashboard
    /// </summary>
    public class DashboardResponse
    {
        /// <summary>
        /// Indicadores da última execução. Nulo quando o projeto ainda não possui relatórios.
        /// </summary>
        public DashboardResumoResponse? Resumo { get; set; }

        /// <summary>
        /// Série temporal das execuções mais recentes, em ordem cronológica
        /// </summary>
        public List<DashboardSerieExecucaoResponse> Series { get; set; } = [];

        /// <summary>
        /// Achados da última execução agrupados por severidade
        /// </summary>
        public List<DashboardSeveridadeResponse> AchadosPorSeveridade { get; set; } = [];

        /// <summary>
        /// Pontuação da última execução por rota auditada
        /// </summary>
        public List<DashboardRotaResponse> PontuacaoPorRota { get; set; } = [];

        /// <summary>
        /// Critérios eMAG mais violados na última execução
        /// </summary>
        public List<DashboardEmagResponse> CriteriosEmag { get; set; } = [];
    }

    /// <summary>
    /// Indicadores da última auditoria do projeto
    /// </summary>
    public class DashboardResumoResponse
    {
        /// <summary>
        /// Data/hora da última execução
        /// </summary>
        public DateTime DataHoraExecucao { get; set; }

        /// <summary>
        /// Pontuação da última execução
        /// </summary>
        public int Pontuacao { get; set; }

        /// <summary>
        /// Diferença de pontuação em relação à execução anterior. Nulo na primeira execução.
        /// </summary>
        public int? VariacaoPontuacao { get; set; }

        /// <summary>
        /// Quantidade de erros da última execução
        /// </summary>
        public int QuantidadeErros { get; set; }

        /// <summary>
        /// Quantidade de avisos da última execução
        /// </summary>
        public int QuantidadeAvisos { get; set; }

        /// <summary>
        /// Quantidade de rotas auditadas na última execução
        /// </summary>
        public int RotasAuditadas { get; set; }

        /// <summary>
        /// Indica se o VLibras foi identificado
        /// </summary>
        public bool TradutorLibrasIdentificado { get; set; }

        /// <summary>
        /// Indica se o Hand Talk foi identificado
        /// </summary>
        public bool HandTalkIdentificado { get; set; }
    }

    /// <summary>
    /// Ponto da série temporal de execuções
    /// </summary>
    public class DashboardSerieExecucaoResponse
    {
        /// <summary>
        /// Data/hora da execução
        /// </summary>
        public DateTime DataHoraExecucao { get; set; }

        /// <summary>
        /// Pontuação da execução
        /// </summary>
        public int Pontuacao { get; set; }

        /// <summary>
        /// Quantidade de erros
        /// </summary>
        public int QuantidadeErros { get; set; }

        /// <summary>
        /// Quantidade de avisos
        /// </summary>
        public int QuantidadeAvisos { get; set; }
    }

    /// <summary>
    /// Quantidade de achados de uma severidade
    /// </summary>
    public class DashboardSeveridadeResponse
    {
        /// <summary>
        /// Nome da severidade exibido na interface
        /// </summary>
        public string Severidade { get; set; } = "";

        /// <summary>
        /// Quantidade de achados
        /// </summary>
        public int Quantidade { get; set; }
    }

    /// <summary>
    /// Pontuação e volume de apontamentos de uma rota
    /// </summary>
    public class DashboardRotaResponse
    {
        /// <summary>
        /// Caminho ou URL da rota
        /// </summary>
        public string Rota { get; set; } = "";

        /// <summary>
        /// Pontuação da rota
        /// </summary>
        public int Pontuacao { get; set; }

        /// <summary>
        /// Quantidade de problemas críticos
        /// </summary>
        public int ProblemasCriticos { get; set; }

        /// <summary>
        /// Quantidade total de apontamentos na rota
        /// </summary>
        public int QuantidadeApontamentos { get; set; }
    }

    /// <summary>
    /// Critério eMAG e quantidade de ocorrências
    /// </summary>
    public class DashboardEmagResponse
    {
        /// <summary>
        /// Código do critério eMAG
        /// </summary>
        public string Criterio { get; set; } = "";

        /// <summary>
        /// Quantidade de apontamentos associados
        /// </summary>
        public int Quantidade { get; set; }
    }
}
