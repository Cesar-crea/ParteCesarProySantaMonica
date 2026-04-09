using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProyectoSantaMonica_Cesar.Models.enums;

namespace ProyectoSantaMonica_Cesar.Models
{
    public class HorariosAtencion
    {
     
        [Key]
        [Column("Id_Horario")]
        public int Id_Horario { get; set; }

     
        [Required(ErrorMessage = "Debe seleccionar un médico")]
        [Column("Id_Medico")]
        public int Id_Medico { get; set; }

       
        [Required(ErrorMessage = "Debe seleccionar un día")]
        [Column("Dia_Semana")]
        public DiaSemana Dia_Semana { get; set; }

      
        [Required(ErrorMessage = "La hora de entrada es obligatoria")]
        [DataType(DataType.Time)]
        [Column("Horario_Entrada")]
        public TimeSpan Horario_Entrada { get; set; }

  
        [Required(ErrorMessage = "La hora de salida es obligatoria")]
        [DataType(DataType.Time)]
        [Column("Horario_Salida")]
        public TimeSpan Horario_Salida { get; set; }

        [ForeignKey("Id_Medico")]
        public Medico? Medico { get; set; }
    }



}
