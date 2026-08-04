public class RelatorioJsonInvalidoException : Exception
{
    public RelatorioJsonInvalidoException(string message, Exception? inner = null)
        : base(message, inner) { }
}
