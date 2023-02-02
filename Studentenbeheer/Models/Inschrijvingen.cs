using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class Inschrijvingen
    {
        public int Id { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public Module? Module { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        

        [Required]
        [DataType(DataType.Date)]
        public DateTime InschrijvingsDatum { get; set; }

        [DataType(DataType.Date)]
        public DateTime? AfgelegdOp { get; set; }

        public string? Resultaat { get; set; }
    }
}
