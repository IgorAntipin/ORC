using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcProto
{
	public static class ContainerConfig
	{

		public static IContainer GetContainer()
		{
			var builder = new ContainerBuilder();



			return builder.Build();
		}
	}
}
