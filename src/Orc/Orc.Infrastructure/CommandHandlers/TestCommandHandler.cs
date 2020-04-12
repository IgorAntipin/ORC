using Orc.Infrastructure.Commands;
using Orc.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.CommandHandlers
{
	/// <summary>
	/// Handler for the test command
	/// </summary>
	public class TestCommandHandler : CommandHandlerBase<TestCommand>
	{
		public TestCommandHandler()
		{
		}

		protected async override Task HandleCommandAsync(TestCommand command)
		{
			if (command.Integer < 0)
				throw new InvalidOperationException($"Test command cannot proceess negative integer! {command.Integer}");


			command.Text = $"Command processed with '{command.Integer}'.";

			await Task.CompletedTask;
		}
	}
}
