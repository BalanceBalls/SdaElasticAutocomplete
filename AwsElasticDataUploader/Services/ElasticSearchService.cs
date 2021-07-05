using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SDAElasticAutoComplete.Exceptions;
using SDAElasticAutoComplete.Models.JsonModels;
using SDAElasticAutoComplete.Repositories;

namespace SDAElasticAutoComplete.Services
{
	public class ElasticSearchService : IElasticSearchService
	{
		private readonly IElasticSearchRepository _elasticSearchRepository;

		public ElasticSearchService(IElasticSearchRepository elasticSearchRepository)
		{
			_elasticSearchRepository = elasticSearchRepository;
		}

		public async Task CreatePropertiesIndexAsync() => await _elasticSearchRepository.CreatePropertiesIndexAsync();

		public async Task CreateManagementCompaniesIndexAsync() => await _elasticSearchRepository.CreateManagementCompaniesIndexAsync();

		public void IndexProperties(IEnumerable<RootProperty> rootProperties, CancellationToken cancellationToken)
		{
			var properties = rootProperties
				.Select(x => x.Property)
				.Distinct()
				.ToList();
			UploadToElastic(properties, cancellationToken);
		}

		public void IndexManagementCompanies(IEnumerable<RootManagementCompany> rootManagementCompanies, CancellationToken cancellationToken)
		{
			var managementCompanies = rootManagementCompanies
				.Select(x => x.ManagementCompany)
				.Distinct()
				.ToList();
			UploadToElastic(managementCompanies, cancellationToken);
		}

		private void UploadToElastic<T>(List<T> dataToUpload, CancellationToken cancellationToken) where T : class
		{
			if (dataToUpload is null || !dataToUpload.Any())
				throw new DataPreparationException("Could not read data from file");

			_elasticSearchRepository.UploadData(dataToUpload, cancellationToken);
		}
	}
}