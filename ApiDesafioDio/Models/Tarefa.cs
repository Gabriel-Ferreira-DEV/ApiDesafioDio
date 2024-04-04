
using System.ComponentModel.DataAnnotations;

namespace TrilhaApiDesafio.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Titulo { get; set; }
        [StringLength(1000,MinimumLength = 5)]
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public EnumStatusTarefa Status { get; set; }

    }
}