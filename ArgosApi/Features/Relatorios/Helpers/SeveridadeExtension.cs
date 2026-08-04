namespace ArgosApi.Features.Relatorios.Helpers
{
    public static class SeveridadeExtension
    {
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
