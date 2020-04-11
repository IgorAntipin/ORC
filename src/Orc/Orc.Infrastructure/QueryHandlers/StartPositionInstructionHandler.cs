using Orc.Domain.Reports;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.QueryHandlers
{
	public class StartPositionInstructionHandler : RobotInstructionQueryHandlerBase<StartPositionInstuction, StatusReport>
	{
		private readonly IControllerFacade _controllerFacade;

		public StartPositionInstructionHandler(IControllerFacade controllerFacade)
		{
			_controllerFacade = controllerFacade ?? throw new ArgumentNullException(nameof(controllerFacade));
		}

		public override async Task<StatusReport> HandleQueryAsync(StartPositionInstuction query)
		{
			var current = await _controllerFacade.GetCurrentPositionAsync();
			var dest = query.AbsolutePosition;
			var offset = dest - current;

			await _controllerFacade.MoveToAsync(offset);

			await _controllerFacade.CleanAsync();

			return new StatusReport()
			{ 
				Status = Common.Enums.RobotInstructionStatus.Ok 
			};
		}
	}
}
