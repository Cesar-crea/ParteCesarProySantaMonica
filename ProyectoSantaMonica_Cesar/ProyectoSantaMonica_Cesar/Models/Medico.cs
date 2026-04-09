using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoSantaMonica_Cesar.Models
{
    public class Medico
    {
        [Key]
        [Column("Id_Medico")]
        public int Id_Medico { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression(@"^\d{8}$")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "El número de colegiatura es obligatorio")]
        [StringLength(20)]
        public string Nro_Colegiatura { get; set; }

        [Required(ErrorMessage = "El número Telefono es obligatorio")]
        [StringLength(9,MinimumLength =7, ErrorMessage ="El telefono debe tener minimo 9 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una especialidad")]
        public int Id_Especialidad { get; set; }

        // solo para mostrar (No en la BD)
        [NotMapped]
        public string? Especialidad { get; set; }
    }
}
