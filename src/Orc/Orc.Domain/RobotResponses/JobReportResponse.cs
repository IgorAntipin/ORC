using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Orc.Domain.RobotResponses
{
	public class JobReportResponse : IRobotResponse, IReportCommand
	{
		/// <summary>
		/// Number of unique cleaned vertices
		/// </summary>
		public int UniqueCleanedVerticesCount { get; set; }

		public TextWriter TextWriter { get; set; }
	}
}
