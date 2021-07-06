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
	public class ManagementCompanyController : ControllerBase
	{
		private readonly IElasticSearchService _elasticSearchService;

		public ManagementCompanyController(IElasticSearchService elasticSearchService)
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
		public IActionResult UploadManagementCompanies([FromForm]IFormFile file, CancellationToken cancellationToken)
		{
			var dataToUpload = file.DeserializeJsonFromFile<List<RootManagementCompany>>();
			_elasticSearchService.IndexManagementCompanies(dataToUpload, cancellationToken);

			return Ok();
		}
	}
}