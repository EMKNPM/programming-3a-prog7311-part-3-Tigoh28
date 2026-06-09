using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace GLMS.Shared
{
	public class Client
	{
		[Key]
		public int Id { get; set; }
		[Required, MaxLength(150)]
		public string Name { get; set; } = string.Empty;
		[Required, MaxLength(100)]
		public string ContactDetails { get; set; } = string.Empty;
		[Required, MaxLength(100)]
		public string Region { get; set; } = string.Empty;

		public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
	}
}
