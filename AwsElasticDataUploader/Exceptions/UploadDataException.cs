using System;

namespace SDAElasticAutoComplete.Exceptions
{
	public class UploadDataException : Exception
	{
		public UploadDataException(string reason) : base($"Error uploading data to elasticsearch : {reason}") { }
	}
}