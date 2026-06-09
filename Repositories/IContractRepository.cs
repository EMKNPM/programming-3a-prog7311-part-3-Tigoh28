using GLMS.Shared;

namespace GLMS.API_.Repositories
{
	public interface IContractRepository
	{
		Task<IEnumerable<Contract>> GetAllContractsAsync(ContractStatus? status, DateTime? startDate, DateTime? endDate);
		Task<Contract?> GetContractByIdAsync(int id);
		Task AddContractAsync(Contract contract);
		Task UpdateContractAsync(Contract contract);
		Task AddServiceRequestAsync(ServiceRequest request);
	}
}
