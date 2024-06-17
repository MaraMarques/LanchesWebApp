using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LanchesWebApp2.Models
{
    public class LancheIngrediente
    {
        public int IngredienteId { get; set; }
        public Ingrediente Ingrediente { get; set; } = null!;

        public int LancheId { get; set; }
        public Lanche Lanche { get; set; } = null!;

    }
}
