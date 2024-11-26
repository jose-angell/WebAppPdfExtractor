using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplicationPDFExtractor.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WebApplicationPDFExtractor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DocumentoModel datosDocumento = new DocumentoModel();
      
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost(IFormFile archivo)
        {
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);

                memoryStream.Position = 0;
                
                using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(memoryStream)))
                {
                    var pageSize = pdfDoc.GetDefaultPageSize();
                    float widtPoints = pageSize.GetWidth();
                    float hidtPoints = pageSize.GetHeight();
                    var region = new Rectangle(230, 500, 80, 200);//200, 650, 200, 200 coordenadas para validar si el docuemto es una constancia fiscal

                    ITextExtractionStrategy strategy = new FilteredTextEventListener(
                        new LocationTextExtractionStrategy(),
                        new TextRegionEventFilter(region)
                        );
                    ITextExtractionStrategy strategySimple = new SimpleTextExtractionStrategy();
                    string data = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1), strategySimple);
                    
                    datosDocumento.RFC = GetDataFromString(data, "FISCAL", "Registro").Replace("\n","");
                    datosDocumento.RazonSocial = GetDataFromString(data, "Contribuyentes", "Nombre,").Replace("\n", "");
                    datosDocumento.Calle = GetDataFromString(data, "Nombre de Vialidad:", "Número Exterior:").Replace("\n", "");
                    datosDocumento.NumeroExterior = GetDataFromString(data, "Número Exterior:", "Número Interior:").Replace("\n", "");
                    datosDocumento.Colonia = GetDataFromString(data, "Nombre de la Colonia:", "Nombre de la Localidad:").Replace("\n", "");
                    datosDocumento.Municipio = GetDataFromString(data, "Nombre del Municipio o Demarcación Territorial:", "Nombre de la Entidad Federativa:").Replace("\n", "");
                    datosDocumento.Estado = GetDataFromString(data, "Nombre de la Entidad Federativa:", "Entre Calle:").Replace("\n", "");
                    datosDocumento.CodigoPostal = GetDataFromString(data, "Código Postal:", "Tipo de Vialidad:").Replace("\n", ""); 
                    datosDocumento.NombreContacto = data;
                }
                
            }
            return Page();
        }
        private string GetDataFromString(string text, string textBefore, string textAfter )
        {
            int posicionInicial = text.IndexOf(textBefore) + textBefore.Length;
            int posicionFinal = text.IndexOf(textAfter);
            int caracteres = posicionFinal - posicionInicial;
            return text.Substring(posicionInicial, caracteres);
        }
    }
}
