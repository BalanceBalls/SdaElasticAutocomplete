using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SdaAutocompleteApi.Services;

namespace SdaAutocompleteApi.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class AutocompleteController : ControllerBase
	{
		private readonly ISearchService _searchService;

		public AutocompleteController(ISearchService searchService)
		{
			_searchService = searchService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Get(string term, string? market, int limit = 25)
		{
			var documents = await _searchService.SearchForTermAsync(term, market, limit);
			return Ok(documents);
		}
	}
}