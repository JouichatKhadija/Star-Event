using System;

namespace StartEvent.Web.Models
{
	public class ContactMessage
	{
		public int Id { get; set; }
		public string Nom { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public DateTime DateEnvoye { get; set; } = DateTime.UtcNow;
	}
}


