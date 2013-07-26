using System;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// Summary description for DataAccessException.
	/// </summary>
	[Serializable]
	public class DataAccessException : ApplicationException
	{
		public DataAccessException() {}

		public DataAccessException(string message) : base(message) {}

		public DataAccessException(string message, Exception e) : base(message, e) {}
	}
}
