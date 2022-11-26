using CQRS.Core.Commads;

namespace Api.Commands;

public class EditCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; } = "";
    public string UserName { get; set; } = "";
}

