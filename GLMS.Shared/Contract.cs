using System.ComponentModel.DataAnnotations;

namespace GLMS.Shared
{
	public class Contract
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int ClientId { get; set; }
		public Client? Client { get; set; }

		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
		[Required]
		public ContractStatus Status { get; set; } = ContractStatus.Draft;
		[Required, MaxLength(50)]
		public string ServiceLevel { get; set; } = "Standard";
		public string? SignedAgreementPath { get; set; }

		public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
	}
}
