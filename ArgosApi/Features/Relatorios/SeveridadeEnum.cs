namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Enumeração de severidade dos apontamentos
    /// </summary>
    public enum SeveridadeEnum
    {
        /// <summary>
        /// Severidade crítica, indica um problema que precisa ser corrigido imediatamente
        /// </summary>
        Critical = 1,
        /// <summary>
        /// Severidade séria, indica um problema que precisa ser corrigido o quanto antes
        /// </summary>
        Serious = 2,
        /// <summary>
        /// Severidade moderada, indica um problema que deve ser corrigido, mas não é urgente
        /// </summary>
        Moderate = 3,
        /// <summary>
        /// Severidade baixa, indica um problema que pode ser corrigido em um momento posterior
        /// </summary>
        Minor = 4,
        /// <summary>
        /// Severidade informativa, indica uma informação relevante, mas que não representa um problema
        /// </summary>
        Info = 5

    }
}
