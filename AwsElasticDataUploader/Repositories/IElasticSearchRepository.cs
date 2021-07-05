using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SDAElasticAutoComplete.Repositories
{
	public interface IElasticSearchRepository
	{
		void UploadData<T>(List<T> dataList, CancellationToken cancellationToken) where T : class;

		Task CreatePropertiesIndexAsync();

		Task CreateManagementCompaniesIndexAsync();
	}
}