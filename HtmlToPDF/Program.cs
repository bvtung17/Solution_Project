using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.StyledXmlParser.Css.Media;

class Program
{
    static void Main()
    {
        string htmlContent = ReadHtmlFile(@"Asset/Customer_Cancelled_Email.html");
        string outputPath = @"C:\Users\Tung\source\repos\bvtung17\Solution_Project\HtmlToPDF\Output\output.pdf";

        GeneratePdfFromHtml(htmlContent, outputPath);

        Console.WriteLine($"PDF file created at: {outputPath}");
    }

    static void GeneratePdfFromHtml(string htmlContent, string outputPath)
    {
        using (FileStream fs = new FileStream(outputPath, FileMode.Create))
        {
            using (PdfWriter writer = new PdfWriter(fs))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);
                    PageSize pageSize = new PageSize(PageSize.A4);// Thiết lập kích thước giấy ngang
                    pdf.SetDefaultPageSize(pageSize);

                    HtmlConverter.ConvertToPdf(htmlContent, pdf, new ConverterProperties().SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.SCREEN)));
                    document.Close();
                }
            }
        }
    }
    static string ReadHtmlFile(string path)
    {
        var emailContent = File.ReadAllText(path);
        return emailContent;
    }
}
