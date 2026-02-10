using System;
using System.ComponentModel.DataAnnotations;

namespace StartEvent.Web.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Email invalide.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le message est requis.")]
        public string Message { get; set; } = string.Empty;

        public DateTime DateEnvoye { get; set; } = DateTime.UtcNow;
    }
}
