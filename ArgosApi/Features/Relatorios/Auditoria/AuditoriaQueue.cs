using System.Threading.Channels;

namespace ArgosApi.Features.Relatorios.Auditoria
{
    /// <summary>
    /// Fila de projetos aguardando execução de auditoria
    /// </summary>
    public class AuditoriaQueue
    {
        private readonly Channel<long> _channel = Channel.CreateUnbounded<long>(
            new UnboundedChannelOptions { SingleReader = true });

        /// <summary>
        /// Enfileira o identificador do projeto para auditoria
        /// </summary>
        public ValueTask EnqueueAsync(long projetoId, CancellationToken cancellationToken = default)
            => _channel.Writer.WriteAsync(projetoId, cancellationToken);

        /// <summary>
        /// Lê os identificadores de projeto enfileirados
        /// </summary>
        public IAsyncEnumerable<long> ReadAllAsync(CancellationToken cancellationToken)
            => _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
