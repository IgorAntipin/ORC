using Orc.Domain.Commands;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.CommandHandlers
{
	public class StartPositionCommandHandler : CommandHandlerBase<StartPositionCommand>
	{
		private readonly IControllerFacade _controllerFacade;

		public StartPositionCommandHandler(IControllerFacade controllerFacade)
		{
			_controllerFacade = controllerFacade ?? throw new ArgumentNullException(nameof(controllerFacade));
		}

		protected override async Task HandleCommandAsync(StartPositionCommand command)
		{
			var current = await _controllerFacade.GetCurrentPositionAsync();
			var dest = command.AbsolutePosition;
			var offset = dest - current;

			await _controllerFacade.MoveToAsync(offset);
			
			if(command.CleanInPosition)
				await _controllerFacade.CleanAsync();
		}
	}
}
