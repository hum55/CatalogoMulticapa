using System.ComponentModel.DataAnnotations;

namespace CatalogoApp.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2)]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no valido")]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        public string Role { get; set; } = "User";

        public DateTime? UltimoLogin { get; set; }
        public DateTime? UltimoLogout { get; set; }
        public int VecesIngreso { get; set; }
    }
}
