using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrcProto.App
{
	public interface IOrcApp
	{
		Task StartAsync(string[] args);
	}
}
