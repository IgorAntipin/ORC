using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IRobotJobBuilder
	{
		void UseReader(TextReader textReader);

		Task<IRobotJob> BuildAsync();
	}
}
