using System.Collections.Generic;

namespace StartEvent.Web.Models
{
	public class AdminDashboardViewModel
	{
		public int TotalEvenements { get; set; }
		public int TotalCelebrites { get; set; }
		public int TotalReservations { get; set; }
		public int TotalMessages { get; set; }

		public IEnumerable<Evenement> Evenements { get; set; } = new List<Evenement>();
		public IEnumerable<Celebrite> Celebrites { get; set; } = new List<Celebrite>();
		public IEnumerable<Reservation> Reservations { get; set; } = new List<Reservation>();
		public IEnumerable<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
	}
}






