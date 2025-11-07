using System;

namespace StartEvent.Web.Models
{
	public class Evenement
	{
		public int Id { get; set; }
		public string Nom { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime DateEvenement { get; set; }
		public string Lieu { get; set; } = string.Empty;
		public string? ImagePath { get; set; }
		public decimal? Prix { get; set; }
		public int? PlacesDisponibles { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string Category { get; set; } = "musique";
	}
}


