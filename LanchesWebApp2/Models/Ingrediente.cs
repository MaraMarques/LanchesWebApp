 using System.ComponentModel.DataAnnotations;

namespace LanchesWebApp2.Models
{
    public class Ingrediente
    {
        [Key]
        public int IngredienteId { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public float PrecoUni {  get; set; }

        public List<Lanche> Lanches { get; set; } = [];
        public List<LancheIngrediente>? LancheIngredientes { get; set; } = [];

    }
}
