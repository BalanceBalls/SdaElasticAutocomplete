﻿using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SDAElasticAutoComplete.Exceptions;

namespace SDAElasticAutoComplete.Extensions
{
	public static class FormFileExtension
	{
		public static T DeserializeJsonFromFile<T>(this IFormFile file)
		{
			using var fileStream = file.OpenReadStream();
			var serializer = new JsonSerializer
			{
				NullValueHandling = NullValueHandling.Ignore
			};
			using var streamReader = new StreamReader(fileStream);
			using var jsonTextReader = new JsonTextReader(streamReader);

			try
			{
				return serializer.Deserialize<T>(jsonTextReader);
			}
			catch (JsonSerializationException e)
			{
				throw new DataPreparationException(e.Message);
			}
		}
	}
}