namespace ArgosApi.Features.Relatorios.Helpers
{
    /// <summary>
    /// Extensões de exibição para severidade
    /// </summary>
    public static class SeveridadeExtension
    {
        /// <summary>
        /// Converte a severidade para o nome exibido na interface
        /// </summary>
        public static string ToDisplayName(this SeveridadeEnum severity) =>
        severity switch
        {
            SeveridadeEnum.Critical => "Crítico",
            SeveridadeEnum.Serious => "Grave",
            SeveridadeEnum.Moderate => "Moderado",
            SeveridadeEnum.Minor => "Baixo",
            SeveridadeEnum.Info => "Informação",
            _ => "Desconhecido"
        };
    }
}
