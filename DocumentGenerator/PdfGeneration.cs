using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace DocumentGenerator
{
    public static class PdfGeneration
    {

        public static void GenerateIdentityCard(string[] MRZ)
        {
            if (MRZ.Length != 3)
            {
                throw new ArgumentException("MRZ should have 3 rows for indentity cards, not : "+MRZ.Length);

            }

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);



            // Create a font
            XFont font = new XFont("OCR-B 10 BT", 12, XFontStyle.Regular);
            var width = XUnit.FromMillimeter(85.68);
            var height = XUnit.FromMillimeter(54.02);
            gfx.DrawRectangle(XBrushes.Beige, new XRect(1, 1, width, height));
            XImage image = XImage.FromFile(@"Templates\IndentityCard_bg.jpg");
            gfx.DrawImage(image, 1, 1, width, height);

            // Draw the text
            gfx.DrawString(MRZ[0], font, XBrushes.Black,
              new XRect(XUnit.FromMillimeter(4), XUnit.FromMillimeter(36.4), XUnit.FromMillimeter(80), 0),
              XStringFormats.Default);

            gfx.DrawString(MRZ[1], font, XBrushes.Black,
             new XRect(XUnit.FromMillimeter(4), XUnit.FromMillimeter(41), XUnit.FromMillimeter(80), 0),
             XStringFormats.Default);

            gfx.DrawString(MRZ[2], font, XBrushes.Black,
              new XRect(XUnit.FromMillimeter(4), XUnit.FromMillimeter(44.5), XUnit.FromMillimeter(80), 0),
              XStringFormats.Default);


            // Save the document...
            const string filename = "IndentityCard.pdf";
            document.Save(filename);
        }
    }
}
