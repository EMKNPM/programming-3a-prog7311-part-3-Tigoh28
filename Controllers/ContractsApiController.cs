using GLMS.API_.Repositories;
using GLMS.API_.Services;
using GLMS.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.API_.Controllers
{
	[ApiController]
	[Route("api/contracts")]
	public class ContractsApiController : ControllerBase
	{
		private readonly IContractRepository _repository;
		private readonly LogisticsService _logisticsService;

		public ContractsApiController(IContractRepository repository, LogisticsService logisticsService)
		{
			_repository = repository;
			_logisticsService = logisticsService;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] ContractStatus? status, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate) =>
			Ok(await _repository.GetAllContractsAsync(status, startDate, endDate));

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Contract contract)
		{
			if (contract.EndDate <= contract.StartDate) return BadRequest("Invalid date bounds.");
			await _repository.AddContractAsync(contract);
			return CreatedAtAction(nameof(Get), new { id = contract.Id }, contract);
		}

		[HttpPatch("{id}/status")]
		public async Task<IActionResult> PatchStatus(int id, [FromBody] ContractStatus newStatus)
		{
			var contract = await _repository.GetContractByIdAsync(id);
			if (contract == null) return NotFound();
			contract.Status = newStatus;
			await _repository.UpdateContractAsync(contract);
			return NoContent();
		}

		[HttpPost("{id}/requests")]
		public async Task<IActionResult> CreateRequest(int id, [FromBody] RequestInputModel input)
		{
			try { return StatusCode(201, await _logisticsService.ProcessServiceRequestAsync(id, input.Description, input.CostInUSD)); }
			catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
		}
	}

	public class RequestInputModel
	{
		public string Description { get; set; } = string.Empty;
		public decimal CostInUSD { get; set; }
	}
}
