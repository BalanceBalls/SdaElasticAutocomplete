using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using SdaCommon.Models;
using SDAElasticAutoComplete.Exceptions;
using SDAElasticAutoComplete.Models.JsonModels;

namespace SDAElasticAutoComplete.Repositories
{
	public class ElasticSearchRepository : IElasticSearchRepository
	{
		private readonly IElasticClient _elasticClient;

		private readonly ILogger<ElasticSearchRepository> _logger;

		private readonly ElasticConfigOptions _elasticConfig;

		public ElasticSearchRepository(IElasticClient elasticClient,IOptions<ElasticConfigOptions> elasticConfig, ILogger<ElasticSearchRepository> logger)
		{
			_elasticClient = elasticClient;
			_elasticConfig = elasticConfig?.Value ?? throw new ArgumentNullException(nameof(elasticConfig));
			_logger = logger;
		}

		public void UploadData<T>(List<T> dataList, CancellationToken cancellationToken) where T : class
		{
			using var waitHandle = new CountdownEvent(1);

			var bulkAll = _elasticClient.BulkAll(dataList, b => b
				.BackOffRetries(2)
				.BackOffTime("30s")
				.MaxDegreeOfParallelism(Environment.ProcessorCount / 2)
				.Size(500)
				, cancellationToken);

			bulkAll.Subscribe(new BulkAllObserver(
				onError: exception => throw new UploadDataException(exception.Message),
				onCompleted:() =>
				{
					waitHandle.Signal();
					_logger.LogInformation("Data uploaded to elastic");
				}));

			waitHandle.Wait(cancellationToken);
		}

		public async Task CreateManagementCompaniesIndexAsync()
		{
			const string analyzerName = "mgmt_analyzer";
			const string searchAnalyzerName = "autocomplete_analyzer";
			var indexName = _elasticConfig.MgmtIndex;
			const int minGram = 2;
			const int maxGram = 11;

			var indexExists = (await _elasticClient.Indices.ExistsAsync(indexName)).Exists;
			if (!indexExists)
			{
				var result = await _elasticClient.Indices.CreateAsync(indexName, index => index
					.Settings(settings => settings
						.Analysis(analysis => analysis
							.TokenFilters(tokenFilters => tokenFilters
								.EdgeNGram("edge_ngram_filter", ngram => ngram
									.MinGram(minGram)
									.MaxGram(maxGram)
								)
								.UserDefined("possessive_english_stemmer", new StemmerTokenFilter
									{
										Language = "possessive_english"
									}
								)
							)
							.Analyzers(analyzers => analyzers
								.Custom(analyzerName, propertyAnalyzer => propertyAnalyzer
									.Tokenizer("standard")
									.Filters(new []
									{
										"lowercase",
										"possessive_english_stemmer",
										"stop",
										"edge_ngram_filter"
									})
								)
								.Custom(searchAnalyzerName, autocompleteAnalyzer => autocompleteAnalyzer
									.Tokenizer("standard")
									.Filters(new []
									{
										"lowercase",
										"stop",
									})
								)
							)
						)
					)
					.Map<ManagementCompany>(mapping => mapping
						.Properties(properties => properties
							.Text(text => text
								.Name(name => name.Name)
								.Analyzer(analyzerName)
								.SearchAnalyzer(searchAnalyzerName)
							)
							.Text(text => text
								.Name(name => name.State)
								.Analyzer(analyzerName)
								.SearchAnalyzer(searchAnalyzerName)
							)
							.Keyword(text => text
								.Name(name => name.Market)
							)
							.Number(number => number
								.Name(name => name.MgmtId)
							)
						)
					)
				);

				if (!result.IsValid)
					throw new IndexCreationFailedException("Failed to create index for management-companies");
			}
		}

		public async Task CreatePropertiesIndexAsync()
		{
			const string analyzerName = "property_analyzer";
			const string searchAnalyzerName = "autocomplete_analyzer";
			var indexName = _elasticConfig.PropertiesIndex;
			const int minGram = 2;
			const int maxGram = 11;

			var indexExists = (await _elasticClient.Indices.ExistsAsync(indexName)).Exists;
			if (!indexExists)
			{
				var result = await _elasticClient.Indices.CreateAsync(indexName, index => index
					.Settings(settings => settings
						.Analysis(analysis => analysis
							.TokenFilters(tokenFilters => tokenFilters
								.EdgeNGram("edge_ngram_filter", ngram => ngram
									.MinGram(minGram)
									.MaxGram(maxGram)
								)
								.UserDefined("possessive_english_stemmer", new StemmerTokenFilter
									{
										Language = "possessive_english"
									}
								)
							)
							.Analyzers(analyzers => analyzers
								.Custom(analyzerName, propertyAnalyzer => propertyAnalyzer
									.Tokenizer("standard")
									.Filters(new []
									{
										"lowercase",
										"possessive_english_stemmer",
										"stop",
										"edge_ngram_filter"
									})
								)
								.Custom(searchAnalyzerName, autocompleteAnalyzer => autocompleteAnalyzer
									.Tokenizer("standard")
									.Filters(new []
									{
										"lowercase",
										"stop",
									})
								)
							)
						)
					)
					.Map<Property>(mapping => mapping
						.Properties(properties => properties
							.Text(text => text
								.Name(name => name.Name)
								.Analyzer(analyzerName)
								.SearchAnalyzer(searchAnalyzerName)
							)
							.Text(text => text
								.Name(name => name.FormerName)
								.Analyzer(analyzerName)
								.SearchAnalyzer(searchAnalyzerName)
							)
							.Text(text => text
								.Name(name => name.StreetAddress)
								.Analyzer(analyzerName)
								.SearchAnalyzer(searchAnalyzerName)
							)
							.Text(text => text
								.Name(name => name.State)
								.Analyzer(analyzerName)
								.SearchAnalyzer(searchAnalyzerName)
							)
							.Text(text => text
								.Name(name => name.City)
								.Analyzer(analyzerName)
								.SearchAnalyzer(searchAnalyzerName)
							)
							.Keyword(text => text
								.Name(name => name.Market)
							)
							.Number(number => number
								.Name(name => name.PropertyId)
							)
						)
					)
				);

				if (!result.IsValid)
					throw new IndexCreationFailedException("Failed to create index for properties");
			}
		}
	}
}