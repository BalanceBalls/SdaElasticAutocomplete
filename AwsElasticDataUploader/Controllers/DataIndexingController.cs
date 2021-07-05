using System.Threading;
using System.Collections.Generic;
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
	public class DataIndexingController : ControllerBase
	{
		private readonly IElasticSearchService _elasticSearchService;

		public DataIndexingController(IElasticSearchService elasticSearchService)
		{
			_elasticSearchService = elasticSearchService;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> CreateIndexForManagementCompanies()
		{
			await _elasticSearchService.CreateManagementCompaniesIndexAsync();
			return Ok();
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

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult UploadManagementCompanies([FromForm]IFormFile file, CancellationToken cancellationToken)
		{
			var dataToUpload = file.DeserializeJsonFromFile<List<RootManagementCompany>>();
			_elasticSearchService.IndexManagementCompanies(dataToUpload, cancellationToken);

			return Ok();
		}
	}
}