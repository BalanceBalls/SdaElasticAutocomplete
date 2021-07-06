using System;

namespace SDAElasticAutoComplete.Exceptions
{
	public class DataPreparationException : Exception
	{
		public DataPreparationException(string reason) : base($"Could not process content from file : {reason}") { }
	}
}