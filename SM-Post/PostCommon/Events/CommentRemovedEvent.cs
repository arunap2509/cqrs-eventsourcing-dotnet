using CQRS.Core.Events;

namespace PostCommon.Events;

public class CommentRemovedEvent : BaseEvent
{
    public CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
    {

    }

    public Guid CommentId { get; set; }

}

