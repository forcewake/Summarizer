using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Text
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TextId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Text")]
        [MaxLength]
        public string FullText { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Short text")]
        [MaxLength]
        public string ShortText { get; set; }

        [Display(Name = "Language")]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        [Display(Name = "Display lines")]
        public int DisplayLines { get; set; }

    }
}