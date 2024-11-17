using System.ComponentModel.DataAnnotations;

namespace WebApplicationPDFExtractor.Models
{
    public class DocumentoModel
    {
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }

        [Display(Name = "Clave de RFC")]
        public string? RFC {  get; set; }

        [Display(Name = "Regimen Fiscal")]
        public string? RegimenFiscal { get; set; }

        [Display(Name = "Clave de Uso")]
        public string? ClavedeUso { get; set; }

        [Display(Name = "Calle")]
        public string? Calle { get; set; }

        [Display(Name = "Numero Exterior")]
        public string? NumeroExterior { get; set; }

        [Display(Name = "Colonia")]
        public string? Colonia { get; set; }

        [Display(Name = "Municipio")]
        public string? Municipio { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [StringLength(maximumLength:5)]
        [Display(Name = "Codigo Postal")]
        public string? CodigoPostal { get; set; }

        [Display(Name = "Pais")]
        public string? Pais { get; set; }

        [RegularExpression(@"^\d{3}-\d{4}-\d{4}$", ErrorMessage = "El formato del telefono debe ser 123-456-7890.")]
        [Display(Name = "Telefono")]
        public string? Telefono { get; set; }

        [Display(Name = "Correo Electronico")]
        public string? CorreoElectronico { get; set; }

        [StringLength(1000)]
        [Display(Name = "Nombre de Contacto")]
        public string? NombreContacto { get; set; }
    }
}
