using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OrcProto.App
{
	public interface IOrcApp
	{
		Task StartAsync(TextReader reader, TextWriter writer);
	}
}
