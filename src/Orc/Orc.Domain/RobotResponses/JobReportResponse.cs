using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotResponses
{
	public class JobReportResponse : IRobotResponse
	{
		/// <summary>
		/// Number of unique cleaned vertices
		/// </summary>
		public int UniqueCleanedVerticesCount { get; set; }
	}
}
