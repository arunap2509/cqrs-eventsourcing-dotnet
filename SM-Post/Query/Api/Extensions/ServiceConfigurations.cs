using Api.Queries;
using CQRS.Core.Infrastructure;
using DomainLayer.Entities;
using InfrastructureLayer.Dispatcher;

namespace Api.Extensions
{
	public static class ServiceConfigurations
	{
		public static void RegisterQueryHandler(this IServiceCollection services)
		{
			var queryHandler = services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
			var dispatcher = new QueryDispatcher();

			// all of them have to have same return type else not working
			dispatcher.RegisterHandler<FindAllPostsQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostsWithLikesQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostWithCommentsQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostByAuthorQuery>(queryHandler.HandleAsync);

			services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);
        }
	}
}

