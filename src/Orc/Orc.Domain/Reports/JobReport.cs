using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Reports
{
	public class JobReport : IRobotReport
	{
		/// <summary>
		/// Number of unique cleaned vertices
		/// </summary>
		public uint UniqueCleanedVerticesCount { get; set; }
	}
}
