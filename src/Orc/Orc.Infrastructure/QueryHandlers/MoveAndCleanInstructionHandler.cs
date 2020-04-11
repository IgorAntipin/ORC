using Orc.Domain.Reports;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.QueryHandlers
{
	public class MoveAndCleanInstructionHandler : RobotInstructionQueryHandlerBase<MoveAndCleanInstruction, StatusReport>
	{
		private readonly IControllerFacade _controllerFacade;

		public MoveAndCleanInstructionHandler(IControllerFacade controllerFacade)
		{
			_controllerFacade = controllerFacade ?? throw new ArgumentNullException(nameof(controllerFacade));
		}

		public override async Task<StatusReport> HandleQueryAsync(MoveAndCleanInstruction query)
		{
			for (int i = 0; i < query.NumberOfSteps; i++)
			{
				await _controllerFacade.MoveToAsync(query.Direction);

				await _controllerFacade.CleanAsync();
			}

			return new StatusReport()
			{
				Status = Common.Enums.RobotInstructionStatus.Ok
			};
		}
	}
}
