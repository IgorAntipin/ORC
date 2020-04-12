using Orc.Domain.Commands;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.CommandHandlers
{
	public class MoveAndCleanCommandHandler : CommandHandlerBase<MoveAndCleanCommand>
	{

		private readonly IControllerFacade _controllerFacade;

		public MoveAndCleanCommandHandler(IControllerFacade controllerFacade)
		{
			_controllerFacade = controllerFacade ?? throw new ArgumentNullException(nameof(controllerFacade));
		}

		protected override async Task HandleCommandAsync(MoveAndCleanCommand command)
		{
			for (int i = 0; i < command.NumberOfSteps; i++)
			{
				await _controllerFacade.MoveToAsync(command.Direction);

				await _controllerFacade.CleanAsync();
			}
		}
	}
}
