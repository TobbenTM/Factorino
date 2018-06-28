using System.ComponentModel.DataAnnotations;

namespace FNO.Domain.Models
{
    public class FactorioEntity
    {
        [Key]
        public string Name { get; set; }

        public string Type { get; set; }

        public string Icon { get; set; }

        public int StackSize { get; set; }

        public string Subgroup { get; set; }

        public bool Fluid { get; set; }
    }
}
