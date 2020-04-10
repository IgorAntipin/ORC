using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Core
{
	public class Processor : IProcessor
	{
		private readonly IServiceProvider _serviceProvider;

		public Processor(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public async Task ExecuteAsync(ICommand command)
		{
			var type = command.GetType();

			Type handlerType = typeof(CommandHandlerBase<>).MakeGenericType(type);
			Type returnHandlerType = typeof(Task);

			var handleMethod = handlerType.GetMethods().FirstOrDefault(m => m.Name == "HandleAsync" && m.ReturnType.FullName == returnHandlerType.FullName);

			if (handleMethod == null)
				throw new InvalidOperationException($"Failed to execute command '{type.FullName}'. Handler not found. Consider registering a proper handler type in DI container.");

			var handler = _serviceProvider.GetService(handlerType);

			if (handler == null)
				throw new InvalidOperationException($"Failed to execute command '{type.FullName}'. Handler not found. Consider registering a proper handler type in DI container.");

			await (Task)handleMethod.Invoke(handler, new object[] { command });
		}


		public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
		{
			var queryType = query.GetType();
			var returnType = typeof(TResult);

			var args = new Type[] { queryType, returnType };
			Type handlerType = typeof(QueryHandlerBase<,>).MakeGenericType(args);
			Type handlerReturnType = typeof(Task<>).MakeGenericType(returnType);

			var methods = handlerType.GetMethods();
			var handleMethod = methods.FirstOrDefault(m => m.Name == "HandleAsync" && m.ReturnType.FullName == handlerReturnType.FullName);
			if (handleMethod == null)
				throw new InvalidOperationException($"Failed to execute query '{queryType.FullName}'. Handler not found. Consider registering a proper handler type in DI container.");

			var handler = _serviceProvider.GetService(handlerType);
			if (handler == null)
				throw new InvalidOperationException($"Failed to execute query '{queryType.FullName}'. Handler not found. Consider registering a proper handler type in DI container.");

			TResult result = await (Task<TResult>)handleMethod.Invoke(handler, new object[] { query });

			return result;
		}
	}
}
