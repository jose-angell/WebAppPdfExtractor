using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;


namespace WebApplicationPDFExtractor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
      
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public async void OnPost(IFormFile archivo)
        {
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);

                memoryStream.Position = 0;
                using (PdfReader pdfRead = new PdfReader(memoryStream))
                {
                    PdfDocument pdfDoc = new PdfDocument(pdfRead);
                    for(int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string data = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                        Debug.WriteLine(data);
                    }
                }
            }

        }
    }
}
