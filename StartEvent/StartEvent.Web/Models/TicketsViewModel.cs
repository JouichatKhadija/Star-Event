using System;
using System.Collections.Generic;

namespace StartEvent.Web.Models
{
	public class TicketItemViewModel
	{
		public string EvenementNom { get; set; } = string.Empty;
		public DateTime DateEvenement { get; set; }
		public string Lieu { get; set; } = string.Empty;
		public string? ImagePath { get; set; }
		public int NombrePlaces { get; set; }
	}

	public class TicketsViewModel
	{
		public string NomComplet { get; set; } = string.Empty;
		public List<TicketItemViewModel> Tickets { get; set; } = new();
	}
}






