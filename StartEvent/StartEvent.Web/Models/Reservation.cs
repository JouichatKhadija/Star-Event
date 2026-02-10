using System;

namespace StartEvent.Web.Models
{
	public class Reservation
	{
		public int Id { get; set; }
		public int UtilisateurId { get; set; }
		public Utilisateur? Utilisateur { get; set; }
		public int EvenementId { get; set; }
		public Evenement? Evenement { get; set; }
		public DateTime DateReservation { get; set; } = DateTime.UtcNow;
		public int NombrePlaces { get; set; } = 1;
		public bool Payee { get; set; } = false;
	}
}


