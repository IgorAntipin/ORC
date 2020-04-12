using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure.Queries
{
	/// <summary>
	/// TestQuery - is used for testing purpose only
	/// </summary>
	public class TestQuery : IQuery<string>
	{
		/// <summary>
		/// Constuctor with test input parameter
		/// </summary>
		/// <param name="code"></param>
		public TestQuery(int code)
		{
			Code = code;
		}
		
		/// <summary>
		/// Contains input parameter of the query
		/// </summary>
		public int Code { get; private set; }

		/// <summary>
		/// Type of the result
		/// </summary>
		public Type ResultType { get { return typeof(string); } }

		public IQuery<string> GetConcreteQuery()
		{
			return this;
		}
	}
}
