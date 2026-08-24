using System.Threading.Channels;

namespace ArgosApi.Features.Relatorios.Auditoria
{
    public class AuditoriaQueue
    {
        private readonly Channel<long> _channel = Channel.CreateUnbounded<long>(
            new UnboundedChannelOptions { SingleReader = true });

        public ValueTask EnqueueAsync(long projetoId, CancellationToken cancellationToken = default)
            => _channel.Writer.WriteAsync(projetoId, cancellationToken);

        public IAsyncEnumerable<long> ReadAllAsync(CancellationToken cancellationToken)
            => _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
