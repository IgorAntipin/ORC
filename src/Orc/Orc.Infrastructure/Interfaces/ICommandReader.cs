﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface ICommandReader
	{
		Task<string> ReadNextAsync();
	}
}
