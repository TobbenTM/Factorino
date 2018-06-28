using System.ComponentModel.DataAnnotations;

namespace FNO.Domain.Models
{
    public class FactorioTechnology
    {
        [Key]
        public string Name { get; set; }

        public string Icon { get; set; }

        public int Level { get; set; }

        public string MaxLevel { get; set; }

        public bool Upgradeable { get; set; }
    }
}
