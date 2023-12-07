using System.ComponentModel.DataAnnotations;

namespace EmpresaSolutions.Models
{
    public class Cargo
    {
        public int CargoId { get; set; }
        public string DescripcionCargo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool FlagModificado { get; set; }
    }
}
