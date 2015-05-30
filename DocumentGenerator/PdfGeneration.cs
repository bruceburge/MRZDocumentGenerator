using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace DocumentGenerator
{
    public static class PdfGeneration
    {

        public static void GenerateIdentityCard(IdentityDocumentModel vm)
        {
            if (vm.MRZ.Length != 3)
            {
                throw new ArgumentException("MRZ should have 3 rows for indentity cards, not : "+vm.MRZ.Length);

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
			var heightMargin = XUnit.FromInch(.25);
			var widthMargin = XUnit.FromInch(.25);
			gfx.DrawRectangle(XBrushes.Beige, new XRect(widthMargin, heightMargin, width, height));
            XImage image = XImage.FromFile(@"Templates\IndentityCard_bg.jpg");
			gfx.DrawImage(image,widthMargin,heightMargin, width, height);

            // Draw the text
            gfx.DrawString(vm.MRZ[0], font, XBrushes.Black,
			  new XRect(XUnit.FromMillimeter(4) + widthMargin, XUnit.FromMillimeter(36.4) + heightMargin, XUnit.FromMillimeter(80), 0),
              XStringFormats.Default);

            gfx.DrawString(vm.MRZ[1], font, XBrushes.Black,
			 new XRect(XUnit.FromMillimeter(4) + widthMargin, XUnit.FromMillimeter(41) + heightMargin, XUnit.FromMillimeter(80), 0),
             XStringFormats.Default);

            gfx.DrawString(vm.MRZ[2], font, XBrushes.Black,
			  new XRect(XUnit.FromMillimeter(4) + widthMargin, XUnit.FromMillimeter(44.5) + heightMargin, XUnit.FromMillimeter(80), 0),
              XStringFormats.Default);

            gfx.DrawLine(XPens.Black, 0, XUnit.FromMillimeter(60) + heightMargin, XUnit.FromInch(8.5), XUnit.FromMillimeter(60) + heightMargin);


            StringBuilder tmpText = new StringBuilder();

            tmpText.AppendLine("DocType : " + vm.DocType);
            tmpText.AppendLine("CountryOfIssue : " + vm.CountryOfIssue);
            tmpText.AppendLine("SurName : " + vm.SurName);
            tmpText.AppendLine("GivenNames : " + vm.GivenNames);
            tmpText.AppendLine("DocumentNum : " + vm.DocumentNum);
            tmpText.AppendLine("Dob : " + vm.Dob);
            tmpText.AppendLine("ExpDate : " + vm.ExpDate);
            tmpText.AppendLine("OptionalOne : " + vm.OptionalOne);
            tmpText.AppendLine("OptionalTwo : " + vm.OptionalTwo);
            tmpText.AppendLine("Nationality : " + vm.Nationality);
            tmpText.AppendLine("Sex : " + vm.Sex);
            tmpText = tmpText.Replace('<', ' ');
            tmpText.AppendLine("MRZ : ");
            tmpText.AppendLine(string.Join("\n", vm.MRZ));
            string text = tmpText.ToString();

            XFont font2 = new XFont("Times New Roman", 10, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            XRect rect = new XRect(widthMargin, XUnit.FromMillimeter(62) + heightMargin, 250, XUnit.FromMillimeter(62) + heightMargin+ 220);
            gfx.DrawRectangle(XBrushes.LightGray, rect);
            //tf.Alignment = ParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            // Save the document...
            //string filename = "IndentityCard_"+DateTime.Now.ToOADate().ToString()+".pdf";
            string filename = "IdentityCard.pdf";
            document.Save(filename);
            System.Diagnostics.Process.Start(filename);
        }
    }
}
