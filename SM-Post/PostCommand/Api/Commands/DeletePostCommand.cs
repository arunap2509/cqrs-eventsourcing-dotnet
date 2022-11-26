using CQRS.Core.Commads;

namespace Api.Commands;

public class DeletePostCommand : BaseCommand
{
    public string UserName { get; set; } = "";
}

