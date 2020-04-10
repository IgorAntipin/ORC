using Orc.Common.Types;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Services
{
	public class InMemoryRobotStore : IRobotStore
	{
		private Vector2d _currentPosition;
		private HashSet<Vector2d> _memory;

		public InMemoryRobotStore()
		{
			_memory = new HashSet<Vector2d>();
		}

		public InMemoryRobotStore(Vector2d startPosition)
			:this()
		{
			_currentPosition = startPosition;
		}

		public async Task AddCleanedPointAsync(Vector2d point)
		{
			await Task.CompletedTask;
			_memory.Add(point);
		}

		public async Task<int> GetCleanedPointsCountAsync()
		{
			await Task.CompletedTask;
			return _memory.Count;
		}

		public async Task<Vector2d> GetCurrentPositionAsync()
		{
			await Task.CompletedTask;
			return _currentPosition;
		}

		public async Task UpdateCurrentPositionAsync(Vector2d absolute)
		{
			await Task.CompletedTask;
			_currentPosition = absolute;
		}
	}
}
