using GLMS.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GLMS.Tests
{
	// WebApplicationFactory spins up an in-memory instance of our API for real HTTP testing
	public class GlmsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;

		public GlmsIntegrationTests(WebApplicationFactory<Program> factory)
		{
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task LiveApiPipeline_GetContracts_ReturnsSuccessAndJsonData()
		{
			// Act
			var response = await _client.GetAsync("api/contracts");

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			var data = await response.Content.ReadFromJsonAsync<List<Contract>>();
			Assert.NotNull(data);
		}

		[Fact]
		public async Task LiveApiPipeline_CreateContractWithInvalidDates_ReturnsBadRequest()
		{
			// Arrange: Contract where EndDate is before StartDate (Invalid Case)
			var invalidContract = new Contract
			{
				ClientId = 1,
				StartDate = DateTime.UtcNow.AddDays(10),
				EndDate = DateTime.UtcNow,
				Status = ContractStatus.Draft,
				ServiceLevel = "Premium"
			};

			// Act
			var response = await _client.PostAsJsonAsync("api/contracts", invalidContract);

			// Assert: Verify that our custom validation rules catch the invalid dates
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
