using Orc.Common.Types;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Services
{
	public class ControllerFacade : IControllerFacade
	{
		private readonly IRobotStore _store;
		private readonly INavigationService _validator;

		public ControllerFacade(IRobotStore store, INavigationService validator)
		{
			_store = store ?? throw new ArgumentNullException(nameof(store));
			_validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}


		public async Task CleanAsync()
		{
			var position = await GetCurrentPosAsync();
			// ... cleaning procedures
			await _store.AddCleanedPointAsync(position);
		}

		public async Task<Vector2d> GetCurrentPositionAsync()
		{
			var position = await GetCurrentPosAsync();
			return position;
		}

		public async Task MoveToAsync(Vector2d relative)
		{
			if (relative.Equals(Vector2d.ZERO))
				return;

			var position = await GetCurrentPosAsync();

			var next = position + relative;

			var canMove = await _validator.CheckPathAsync(position, next);
			if(!canMove)
			{
				throw new InvalidOperationException($"Failed to find path form {position} to {next}.");
			}
			// ...use engines and get to the destination point
			await _store.UpdateCurrentPositionAsync(next);
		}

		private async Task<Vector2d> GetCurrentPosAsync()
		{
			return await _store.GetCurrentPositionAsync(); 
		}
	}
}
