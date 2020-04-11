using Orc.Domain.Interfaces;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure.QueryHandlers
{
	public abstract class RobotInstructionQueryHandlerBase<TQuery, TResult> : QueryHandlerBase<TQuery, TResult>
		where TQuery : BaseRobotInstruction<TResult> where TResult : IRobotReport
	{

	}
}
