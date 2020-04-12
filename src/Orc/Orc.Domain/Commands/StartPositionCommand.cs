using Orc.Common.Types;
using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Commands
{
	public class StartPositionCommand : ICommand
	{
		public StartPositionCommand(Vector2d absolute, bool clean)
		{
			AbsolutePosition = absolute;
			CleanInPosition = clean;
		}

		public Vector2d AbsolutePosition { get; set; }

		public bool CleanInPosition { get; set; }
	}
}
