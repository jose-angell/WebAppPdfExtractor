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
                    var region = new Rectangle(120, 600, 200, 200);
                    
                    ITextExtractionStrategy strategy = new FilteredTextEventListener(
                        new LocationTextExtractionStrategy(),
                        new TextRegionEventFilter(region)
                        );
                    
                    string data = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1), strategy);

                    datosDocumento.RazonSocial = data;
                }
                
            }
            return Page();
        }
    }
}
