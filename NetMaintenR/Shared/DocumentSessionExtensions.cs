using Marten;

namespace NetMaintenR.Shared;

public static class DocumentSessionExtensions
{
    public static Task Decide<TEntity, TCommand, TEvent>(
        this IDocumentSession session,
        Func<TCommand, TEntity, TEvent[]> decide,
        Func<TEntity> getDefault,
        Guid streamId,
        TCommand command,
        CancellationToken ct = default)
        where TEntity : class =>
            session.Events.WriteToAggregate<TEntity>(streamId, stream =>
                stream.AppendMany(decide(command, stream.Aggregate ?? getDefault()).Cast<object>().ToArray()), ct);
}