using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SDAElasticAutoComplete.Extensions;
using SDAElasticAutoComplete.Models.JsonModels;
using SDAElasticAutoComplete.Services;

namespace SDAElasticAutoComplete.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class PropertyController : ControllerBase
	{
		private readonly IElasticSearchService _elasticSearchService;

		public PropertyController(IElasticSearchService elasticSearchService)
		{
			_elasticSearchService = elasticSearchService;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> CreateIndexForProperties()
		{
			await _elasticSearchService.CreatePropertiesIndexAsync();
			return Ok();
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult UploadProperties([FromForm]IFormFile file, CancellationToken cancellationToken)
		{
			var dataToUpload = file.DeserializeJsonFromFile<List<RootProperty>>();
			_elasticSearchService.IndexProperties(dataToUpload, cancellationToken);

			return Ok();
		}
	}
}