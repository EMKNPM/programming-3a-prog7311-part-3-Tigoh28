using GLMS.API_.Data;
using GLMS.Shared;
using Microsoft.EntityFrameworkCore;

namespace GLMS.API_.Repositories
{
	public class ContractRepository : IContractRepository
	{
		private readonly ApplicationDbContext _context;
		public ContractRepository(ApplicationDbContext context) => _context = context;

		public async Task<IEnumerable<Contract>> GetAllContractsAsync(ContractStatus? status, DateTime? startDate, DateTime? endDate)
		{
			IQueryable<Contract> query = _context.Contracts.Include(c => c.Client);
			if (status.HasValue) query = query.Where(c => c.Status == status.Value);
			if (startDate.HasValue && endDate.HasValue) query = query.Where(c => c.StartDate >= startDate.Value && c.EndDate <= endDate.Value);
			return await query.ToListAsync();
		}

		public async Task<Contract?> GetContractByIdAsync(int id) => await _context.Contracts.Include(c => c.Client).Include(c => c.ServiceRequests).FirstOrDefaultAsync(c => c.Id == id);
		public async Task AddContractAsync(Contract contract) { await _context.Contracts.AddAsync(contract); await _context.SaveChangesAsync(); }
		public async Task UpdateContractAsync(Contract contract) { _context.Contracts.Update(contract); await _context.SaveChangesAsync(); }
		public async Task AddServiceRequestAsync(ServiceRequest request) { await _context.ServiceRequests.AddAsync(request); await _context.SaveChangesAsync(); }
	}
}
