using System;

namespace SDAElasticAutoComplete.Exceptions
{
	public class IndexCreationFailedException : Exception
	{
		public IndexCreationFailedException(string reason) : base($"Failed to create index : {reason}") { }
	}
}