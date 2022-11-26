using CQRS.Core.Events;

namespace CQRS.Core.Domain;

public abstract class AggregateRoot
{
    private readonly List<BaseEvent> _changes = new();

    public Guid Id { get; protected set; }

    public int Version { get; set; } = -1;

    public IEnumerable<BaseEvent> GetUncommitedChanges()
    {
        return _changes;
    }

    public void MakeChangesCommited()
    {
        _changes.Clear();
    }

    private void ApplyChanges(BaseEvent @event, bool isNew)
    {
        var method = GetType().GetMethod("Apply", new Type[] { @event.GetType() });

        if (method == null)
        {
            throw new ArgumentNullException(nameof(method), $"the apply method was not found in the aggregate for {@event.GetType().Name}");
        }

        method.Invoke(this, new object[] { @event });

        if (isNew)
        {
            _changes.Add(@event);
        }
    }

    public void RaiseEvent(BaseEvent @event)
    {
        ApplyChanges(@event, true);
    }

    public void ReplyEvents(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChanges(@event, false);
        }
    }
}

