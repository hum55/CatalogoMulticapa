namespace CatalogoApp.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public int Calificacion { get; set; }
        public string Comentario { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
