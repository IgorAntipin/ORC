using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Core
{
	public abstract class CommandHandlerBase<TCommand> : ICommandHandler
			where TCommand : class, ICommand
	{

		public virtual bool CanHandle(ICommand command)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));
			return command is TCommand;
		}

		public async Task HandleAsync(ICommand command)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));
			var concrete = GetConcreteCommand(command);

			await HandleCommandAsync(concrete);
		}

		protected TCommand GetConcreteCommand(ICommand command)
		{
			var concrete = command as TCommand;
			if (concrete == null)
				throw new InvalidOperationException($"Query handler with type {this.GetType()} can't process query with type { command.GetType()}. {typeof(TCommand)} expected.");
			return concrete;
		}

		protected abstract Task HandleCommandAsync(TCommand command);
	}
}
