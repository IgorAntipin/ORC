using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotInstructions
{
	public class BaseRobotInstruction<TResult> : IRobotInstruction, IQuery<TResult>
		where TResult : IRobotReport
	{
		public Type ResultType { get { return typeof(TResult); } }
	}
}
