using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SDAElasticAutoComplete.Models.JsonModels;

namespace SDAElasticAutoComplete.Services
{
	public interface IElasticSearchService
	{
		Task CreatePropertiesIndexAsync();

		Task CreateManagementCompaniesIndexAsync();

		public void IndexProperties(IEnumerable<RootProperty> rootProperties, CancellationToken cancellationToken);

		public void IndexManagementCompanies(IEnumerable<RootManagementCompany> rootManagementCompanies, CancellationToken cancellationToken);
	}
}