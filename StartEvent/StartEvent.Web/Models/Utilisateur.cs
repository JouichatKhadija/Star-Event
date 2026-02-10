namespace StartEvent.Web.Models
{
	public class Utilisateur
	{
		public int Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Role { get; set; } = "utilisateur";
	}
}


