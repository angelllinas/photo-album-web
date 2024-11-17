using Mysqlx;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoAlbumWeb.Models
{
    [Table("Photo_board")]
    public class PhotoBoard
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        
        [Column("photo")]
        public byte[]? Photo { get; set; }

        [Required(ErrorMessage = "The photo description is required.")]
        [Column("photo_description")]
        [StringLength(100, MinimumLength = 25, ErrorMessage = "La descripción de la foto debe tener entre 25 y 100 caracteres.")]
        public string? PhotoDescription { get; set; }

        [Required(ErrorMessage = "The event date is required.")]
        [Column("event_date", TypeName = "DATE")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "The event location is required.")]
        [Column("location")]
        [StringLength(255)]
        public string? Location { get; set; }

        [Required(ErrorMessage = "The event description is required.")]
        [Column("event_description")]
        [StringLength(100, MinimumLength = 25, ErrorMessage = "La descripción de la foto debe tener entre 25 y 100 caracteres.")]
        public string? EventDescription { get; set; }
    }
}
