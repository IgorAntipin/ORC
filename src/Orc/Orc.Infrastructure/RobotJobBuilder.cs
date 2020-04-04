using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure
{
	public class RobotJobBuilder : IRobotJobBuilder
	{
		private string[] _arguments;

		public RobotJobBuilder()
		{

		}

		public IRobotJob Build()
		{
			return null;
		}

		public void UseArguments(string[] args)
		{
			_arguments = args;
		}
	}
}
