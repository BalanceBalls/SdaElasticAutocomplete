using System;

namespace SdaAutocompleteApi.Exceptions
{
	public class SearchFailedException : Exception
	{
		public SearchFailedException(string reason) : base($"Failed to execute search : {reason}") { }
	}
}