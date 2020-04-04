using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure.Interfaces
{
	public interface IRobotJobBuilder
	{
		void UseArguments(string[] args);

		IRobotJob Build();
	}
}
