using CQRS.Core.Commads;

namespace Api.Commands;

public class EditPostCommand : BaseCommand
{
    public string Message { get; set; } = "";
}

