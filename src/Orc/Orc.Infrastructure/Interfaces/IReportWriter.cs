using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Methods for a report writer
	/// </summary>
	public interface IReportWriter
	{
		/// <summary>
		/// Set TextWriter as Report Writing output
		/// </summary>
		/// <param name="textWriter"></param>
		void UseTextWriter(TextWriter textWriter);

		/// <summary>
		/// Writes report data using TextWriter
		/// </summary>
		/// <param name="report"></param>
		/// <returns></returns>
		Task WriteAsync(IRobotResponse report);
	}
}
