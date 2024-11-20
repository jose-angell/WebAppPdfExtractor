using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplicationPDFExtractor.Models;


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
                    
                    string data = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1), strategy);
                    int posicionInicial = data.IndexOf("FISCAL") + "FISCAL".Length;
                    int posicionFinal = data.IndexOf("Registro");
                    int caracteres = posicionFinal - posicionInicial;
                    string rfctext = data.Substring(posicionInicial, caracteres);
                    datosDocumento.RFC = rfctext;
                    datosDocumento.RazonSocial = data;
                }
                
            }
            return Page();
        }
    }
}
