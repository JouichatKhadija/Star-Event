namespace StartEvent.Web.Models
{
	public class PaymentViewModel
	{
		public string Nom { get; set; } = string.Empty;
		public string Prenom { get; set; } = string.Empty;
		public string NumeroCarte { get; set; } = string.Empty;
		public string TypeCarte { get; set; } = string.Empty;
		public decimal Total { get; set; }
	}
}






