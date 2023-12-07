using System.ComponentModel.DataAnnotations;

namespace EmpresaSolutions.Models
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public int RUC { get; set; }
        public string RazonSocial { get; set; }
        public DateTime FechaFundacion { get; set; }
        public bool Estado { get; set; }
    }
}
