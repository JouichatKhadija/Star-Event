using System;
using System.Collections.Generic;

namespace StartEvent.Web.Models
{
    public class HomeViewModel
    {
        public List<Celebrite> Celebrites { get; set; } = new List<Celebrite>();
        public List<Evenement> Evenements { get; set; } = new List<Evenement>();
        public ContactFormViewModel ContactForm { get; set; } = new ContactFormViewModel();
    }

}
