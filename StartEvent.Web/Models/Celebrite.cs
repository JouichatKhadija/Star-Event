using System;

namespace StartEvent.Web.Models
{
	public class Celebrite
	{
		public int Id { get; set; }
		public string Nom { get; set; } = string.Empty;
		public string? PhotoPath { get; set; }
		public DateTime? DateNaissance { get; set; }
		public string? Profession { get; set; }
		public string? LieuNaissance { get; set; }
		public string? Parcours { get; set; }
		public string? Loisirs { get; set; }
	}
}


