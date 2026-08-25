/// <summary>
/// Exceção lançada quando o JSON do relatório é inválido
/// </summary>
public class RelatorioJsonInvalidoException : Exception
{
    /// <summary>
    /// Inicializa a exceção com a mensagem e a causa, quando houver
    /// </summary>
    public RelatorioJsonInvalidoException(string message, Exception? inner = null)
        : base(message, inner) { }
}
