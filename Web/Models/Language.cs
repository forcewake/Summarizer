using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LanguageId { get; set; }

        [Display(Name = "Full language name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Short language name")]
        public string ShortName { get; set; }
    }
}