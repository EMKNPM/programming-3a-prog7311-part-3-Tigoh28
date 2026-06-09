using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Shared
{
	public class ServiceRequest
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int ContractId { get; set; }
		public Contract? Contract { get; set; }

		[Required, MaxLength(1000)]
		public string Description { get; set; } = string.Empty;

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal CostInUSD { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal CostInZAR { get; set; }

		[Required]
		public ServiceStatus Status { get; set; } = ServiceStatus.Pending;
	}
}
