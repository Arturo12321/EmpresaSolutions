using System.ComponentModel.DataAnnotations;

namespace EmpresaSolutions.Models
{
    public class Personal
    {
        [Key]
        public int EmpleadoId { get; set; }
        public int Documento { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public int CargoId { get; set; }
        public Cargo Cargo { get; set; }
        public bool Active{ get; set; }
    }
}
