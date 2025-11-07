using Microsoft.EntityFrameworkCore;
using StartEvent.Web.Models;

namespace StartEvent.Web.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Celebrite> Celebrites => Set<Celebrite>();
		public DbSet<Evenement> Evenements => Set<Evenement>();
		public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
		public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
		public DbSet<Reservation> Reservations => Set<Reservation>();
	
	
	
	
	
	}
}


