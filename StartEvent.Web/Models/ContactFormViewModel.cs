using System.ComponentModel.DataAnnotations;

namespace StartEvent.Web.Models
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Le nom est requis.")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Email invalide.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le message est requis.")]
        public string Message { get; set; } = string.Empty;
    }
}
