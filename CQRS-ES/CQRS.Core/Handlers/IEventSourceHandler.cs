using CQRS.Core.Domain;

namespace CQRS.Core.Handlers;

public interface IEventSourceHandler<T>
{
    Task SaveAsync(AggregateRoot aggregate);
    Task<T> GetByIdAsync(Guid id);
    Task RepublishEventsAsync();
}

