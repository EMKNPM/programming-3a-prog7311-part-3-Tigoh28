using GLMS.API_.Repositories;
using GLMS.Shared;
using System.Text.Json;

namespace GLMS.API_.Services
{
	public class LogisticsService
	{
		private readonly IContractRepository _repository;
		private readonly HttpClient _httpClient;

		public LogisticsService(IContractRepository repository, HttpClient httpClient)
		{
			_repository = repository;
			_httpClient = httpClient;
		}

		public async Task<decimal> GetUsdToZarRateAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("https://er-api.com");
				if (!response.IsSuccessStatusCode) return 18.50m;
				var jsonString = await response.Content.ReadAsStringAsync();
				using var doc = JsonDocument.Parse(jsonString);
				return doc.RootElement.GetProperty("rates").GetProperty("ZAR").GetDecimal();
			}
			catch { return 18.50m; }
		}

		public async Task<ServiceRequest> ProcessServiceRequestAsync(int contractId, string description, decimal costInUsd)
		{
			var contract = await _repository.GetContractByIdAsync(contractId);
			if (contract == null) throw new KeyNotFoundException("Contract not found.");
			if (contract.Status == ContractStatus.Expired || contract.Status == ContractStatus.OnHold)
				throw new InvalidOperationException("Contract is inactive.");

			decimal rate = await GetUsdToZarRateAsync();
			var newRequest = new ServiceRequest
			{
				ContractId = contractId,
				Description = description,
				CostInUSD = costInUsd,
				CostInZAR = costInUsd * rate,
				Status = ServiceStatus.Pending
			};
			await _repository.AddServiceRequestAsync(newRequest);
			return newRequest;
		}
	}
}
