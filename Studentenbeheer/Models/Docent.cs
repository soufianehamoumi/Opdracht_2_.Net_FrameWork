using Studentenbeheer.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class Docent
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }

        [DataType (DataType.Date)]
        public DateTime Geboortedatum { get; set; }
        [ForeignKey("Gender")]
        [Display(Name = "GenderId")]
        public char GenderID { get; set; }
        public Gender? Gender { get; set; }


        

        [Display(Name = "ModuleIds")]
        [NotMapped]
        public List<int> ModuleIds { get; set; }



        [Display(Name = "Modules")]
        public List<Module>? Modules { set; get; }


        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        [ForeignKey("StudentenbeheerUser")]
        [Display(Name = "UserId")]
        public string UserId { get; set; } = "-";

        public StudentenbeheerUser? user { get; set; }
    }
}
