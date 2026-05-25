using System.ComponentModel.DataAnnotations;

namespace CatalogoApp.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La posicion es obligatoria")]
        public string Posicion { get; set; } = "";

        [Required(ErrorMessage = "La posicion corta es obligatoria")]
        [StringLength(10)]
        public string PosicionCorta { get; set; } = "";

        [Required(ErrorMessage = "El pais es obligatorio")]
        public string Pais { get; set; } = "";

        [StringLength(5)]
        public string PaisCodigo { get; set; } = "";

        [StringLength(10)]
        public string Bandera { get; set; } = "";

        [Required(ErrorMessage = "El equipo es obligatorio")]
        public string Equipo { get; set; } = "";

        [Range(0, 2000, ErrorMessage = "Goles debe estar entre 0 y 2000")]
        public int Goles { get; set; }

        [Range(0, 2000, ErrorMessage = "Asistencias debe estar entre 0 y 2000")]
        public int Asistencias { get; set; }

        [Range(0, 3000, ErrorMessage = "Partidos debe estar entre 0 y 3000")]
        public int Partidos { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; } = "";

        public string FechaNacimiento { get; set; } = "";

        [Range(100, 220, ErrorMessage = "Altura debe estar entre 100 y 220 cm")]
        public int AlturaCm { get; set; }

        [Range(40, 130, ErrorMessage = "Peso debe estar entre 40 y 130 kg")]
        public int PesoKg { get; set; }

        [Range(1, 99, ErrorMessage = "Numero de camiseta debe estar entre 1 y 99")]
        public int NumeroCamiseta { get; set; }

        public string PieDominante { get; set; } = "";

        public string Clubes { get; set; } = "";

        public string FotoUrl { get; set; } = "";

        public string FotoPosition { get; set; } = "";

        public string FotoDetalleUrl { get; set; } = "";

        public string FotoDetallePosition { get; set; } = "";

        [Required]
        public string Genero { get; set; } = "Futbol";
    }
}
