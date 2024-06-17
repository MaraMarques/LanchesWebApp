using System.ComponentModel.DataAnnotations;

namespace LanchesWebApp2.Models
{
    public class Lanche
    {
        [Key]
        public int LancheId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public float Preco {  get; set; }

        public List<int> IngredienteIds { get; set; }
        public List<Ingrediente> Ingredientes { get; set; } = [];
        public List<LancheIngrediente>? LancheIngredientes { get; set; } = [];
    }
}
