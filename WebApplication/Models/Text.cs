using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Text
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TextId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Text")]
        public string FullText { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Short text")]
        public string ShortText { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }


        [Required]
        [Display(Name = "Display linest")]
        public int DisplayLines { get; set; }

    }
}