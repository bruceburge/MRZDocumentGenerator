using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace DocumentGenerator
{
    public partial class Form1 : Form
    {
        List<string> _givenNamesList = new List<string>();
        List<string> _surNamesList = new List<string>();
        static Random rnd = new Random();
        static string _pathToFont = Environment.CurrentDirectory + @"\Fonts\OCR-B 10 BT.ttf";
        static string _pathToNames = Environment.CurrentDirectory + @"\data\names.xml";
        public Form1()
        {
            InitializeComponent();
            btnRandom.Enabled = false;
            btnInstallFont.Enabled = false;
        }


        private bool IsFontInstalled(string fontName)
        {
            using (var testFont = new Font(fontName, 8))
            {
                return 0 == string.Compare(
                  fontName,
                  testFont.Name,
                  StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            PrivateFontCollection pfc = new PrivateFontCollection(); // font collection

            if (!IsFontInstalled("OCR-B 10 BT"))
            {
                if (System.IO.File.Exists(_pathToFont))
                {
                    pfc.AddFontFile(Environment.CurrentDirectory + "\\Fonts\\OCR-B 10 BT.ttf"); // load font
                    rtbMRZ.Font = new Font(pfc.Families[0], 14.0f, FontStyle.Regular);
                    rtbMRZ.AppendText("MRZ Will Be Generated Here");
                }
                else
                {
                    MessageBox.Show("OCR B font not installed, or found at " + Environment.NewLine + _pathToFont + Environment.NewLine + "The application will work but display will be in wrong font");
                }
            }
            else
            {
                rtbMRZ.Font = new Font("OCR-B 10 BT", 14.0f, FontStyle.Regular); 
            }
            //Hide Green Card until done.
            //tabControl1.TabPages.RemoveByKey("tbIndentityCard");

            cmbNationality.DataSource = Enum.GetValues(typeof(DocumentBase.CountryCode));
            cmbNationality.SelectedItem = DocumentBase.CountryCode.USA;
            cmbIssuer.DataSource = Enum.GetValues(typeof(DocumentBase.CountryCode));
            cmbIssuer.SelectedItem = DocumentBase.CountryCode.USA;
            cmbSex.DataSource = Enum.GetValues(typeof(DocumentBase.SexCode));

            
            cmbIdentNationality.DataSource = Enum.GetValues(typeof(DocumentBase.CountryCode));
            cmbIdentNationality.SelectedItem = DocumentBase.CountryCode.USA;
            cmbIdentIssuer.DataSource = Enum.GetValues(typeof(DocumentBase.CountryCode));
            cmbIdentIssuer.SelectedItem = DocumentBase.CountryCode.USA;
            cmbIdentSex.DataSource = Enum.GetValues(typeof(DocumentBase.SexCode));


            if (System.IO.File.Exists(_pathToNames))
            {
                btnRandom.Enabled = true;
                //Random rnd = new Random();
                var selectedNode = XDocument.Load(_pathToNames);
                _givenNamesList = selectedNode.Descendants("GivenNames").Elements("name").Select(x => x.Value).Where(s => s != string.Empty).ToList();
                _surNamesList = selectedNode.Descendants("Surnames").Elements("name").Select(x => x.Value).Where(s => s != string.Empty).ToList();
            }
            else
            {
                MessageBox.Show("Could not load names data from " + Environment.NewLine + _pathToNames+Environment.NewLine+"Random generation disabled");
            }

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            DocumentBase.CountryCode nationality;
            Enum.TryParse<DocumentBase.CountryCode>(cmbNationality.SelectedValue.ToString(), out nationality);

            DocumentBase.CountryCode issuer;
            Enum.TryParse<DocumentBase.CountryCode>(cmbIssuer.SelectedValue.ToString(), out issuer);

            DocumentBase.SexCode sex;
            Enum.TryParse<DocumentBase.SexCode>(cmbSex.SelectedValue.ToString(), out sex);

            PassportModel tmp = new PassportModel(
                nationality,
                charRemap(txtSurname.Text.Replace(' ', '<')),
                charRemap(txtGivenName.Text.Replace(' ', '<')),
                txtPassportNumber.Text,
                dtpBirth.Value.ToString("yyMMdd"),
                dtpExpire.Value.ToString("yyMMdd"),
                charRemap(txtPersonalNumber.Text.Replace(' ', '<')),
                "ValueNotUsed",
                issuer,
                sex,
                txtType.Text
                );

            rtbMRZ.Text = MrzGeneration.GeneratePassportMRZ(tmp);

        }

        private void btnRandom_Click(object sender, EventArgs e)
        {

            txtSurname.Text = !cbkSurname.Checked ? _surNamesList[rnd.Next(_surNamesList.Count)] : txtSurname.Text;
            txtGivenName.Text = !cbkGivenName.Checked ? (_givenNamesList[rnd.Next(_givenNamesList.Count)] + " " + _givenNamesList[rnd.Next(_givenNamesList.Count)]) : txtGivenName.Text;
            cmbIssuer.SelectedIndex = !cbkIssuer.Checked ? rnd.Next(cmbIssuer.Items.Count) : cmbIssuer.SelectedIndex;
            cmbNationality.SelectedIndex = !cbkNationality.Checked ? rnd.Next(cmbNationality.Items.Count) : cmbNationality.SelectedIndex;
            cmbSex.SelectedIndex = !cbkSex.Checked ? rnd.Next(cmbSex.Items.Count) : cmbSex.SelectedIndex;
            dtpBirth.Value = !cbkDOB.Checked ? GetRandomDate(DateTime.Now.AddYears(-100), DateTime.Now.AddMonths(-1)) : dtpBirth.Value;
            dtpExpire.Value = !cbkDOE.Checked ? GetRandomDate(DateTime.Now, DateTime.Now.AddYears(10)) : dtpExpire.Value;
            txtPassportNumber.Text = !cbkPassport.Checked ? rnd.Next(999999999).ToString().PadRight(9, '0') : txtPassportNumber.Text;
            txtPersonalNumber.Text = !cbkPersonalNumber.Checked ? GetRandomString(14) : txtPersonalNumber.Text;
            btnGenerate_Click(null, null);
        }

        private static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }

        private static string GetRandomString(int length)
        {
            string path = string.Empty;

            while (path.Length < length)
            {
                path += Path.GetRandomFileName();
                path = path.Replace(".", "");
            }
            return path.Substring(0, rnd.Next(length)).PadRight(length, ' ');

        }

        private static string charRemap(string input)
        {
            string output = input
            .Replace("Ä", "AE")
            .Replace("Å", "AA")
            .Replace("Æ", "AE")
            .Replace("Ĳ", "IJ")
            .Replace("Ñ", "N")
            .Replace("Ä", "AE")
            .Replace("Ö", "OE")
            .Replace("Ø", "OE")
            .Replace("Ü", "UE")
            .Replace("ß", "SS");
            return output;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tbInfo"])
            {
                if (!IsFontInstalled("OCR-B 10 BT"))
                {
                    lblFontMessage.Text = "OCR-B 10 BT does not appear to be installed, you can optionally install it by click button below to install";
                    btnInstallFont.Enabled = true;
                }
                else
                {
                    lblFontMessage.Text = "OCR-B 10 BT appears to be installed";
                    btnInstallFont.Enabled = false;

                }
            }
        }

        private void btnInstallFont_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_pathToFont);
            btnInstallFont.Enabled = false;
            lblFontMessage.Text = "After font is installed please restart application";
        }

        private void lnkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnkLabel.Text);
        }

        private void btnIdentGenerate_Click(object sender, EventArgs e)
        {
            DocumentBase.CountryCode nationality;
            Enum.TryParse<DocumentBase.CountryCode>(cmbIdentNationality.SelectedValue.ToString(), out nationality);

            DocumentBase.CountryCode issuer;
            Enum.TryParse<DocumentBase.CountryCode>(cmbIdentIssuer.SelectedValue.ToString(), out issuer);

            DocumentBase.SexCode sex;
            Enum.TryParse<DocumentBase.SexCode>(cmbIdentSex.SelectedValue.ToString(), out sex);

            
            
            IdentityDocumentModel tmp = new IdentityDocumentModel(
				issuer,
                charRemap(txtIdentSurname.Text.Replace(' ', '<')),
                charRemap(txtIdentGiven.Text.Replace(' ', '<')),
                txtIdentDocNum.Text,
                dtpIdentDOB.Value.ToString("yyMMdd"),
                dtpIdentDOE.Value.ToString("yyMMdd"),
                charRemap(txtIdentOptionalOne.Text.Replace(' ', '<')),
                charRemap(txtIdentOptionalTwo.Text.Replace(' ', '<')),
                "",
                nationality,
                sex,
                txtIdentDocType.Text
                );

            var MRZ = MrzGeneration.GenerateIdentityCardMRZ(tmp);
            tmp.MRZ = MRZ;
            PdfGeneration.GenerateIdentityCard(tmp);
            rtbMRZ.Text = MRZ[0]+Environment.NewLine+MRZ[1]+Environment.NewLine+MRZ[2];
        }

        private void btnIdentGenerateRandom_Click(object sender, EventArgs e)
        {
            txtIdentSurname.Text = !cbkIdentSurname.Checked ? _surNamesList[rnd.Next(_surNamesList.Count)] : txtIdentSurname.Text;
            txtIdentGiven.Text = !cbkIdentGivenName.Checked ? (_givenNamesList[rnd.Next(_givenNamesList.Count)] + " " + _givenNamesList[rnd.Next(_givenNamesList.Count)]) : txtIdentGiven.Text;
            cmbIdentIssuer.SelectedIndex = !cbkIdentIssuer.Checked ? rnd.Next(cmbIssuer.Items.Count) : cmbIdentIssuer.SelectedIndex;
            cmbIdentNationality.SelectedIndex = !cbkIdentNationality.Checked ? rnd.Next(cmbNationality.Items.Count) : cmbIdentNationality.SelectedIndex;
            cmbIdentSex.SelectedIndex = !cbkIdentSex.Checked ? rnd.Next(cmbSex.Items.Count) : cmbIdentSex.SelectedIndex;
            dtpIdentDOB.Value = !cbIdentkDOB.Checked ? GetRandomDate(DateTime.Now.AddYears(-100), DateTime.Now.AddMonths(-1)) : dtpIdentDOB.Value;
            dtpIdentDOE.Value = !cbkIdentDOE.Checked ? GetRandomDate(DateTime.Now, DateTime.Now.AddYears(10)) : dtpIdentDOE.Value;
            txtIdentDocNum.Text = !cbkIdentDocNum.Checked ? rnd.Next(999999999).ToString().PadRight(9, '0') : txtIdentDocNum.Text;
            txtIdentOptionalOne.Text = !cbkIdentOptionalOne.Checked ? GetRandomString(15) : txtIdentOptionalOne.Text;
            txtIdentOptionalTwo.Text = !cbkIdentOptionalTwo.Checked ? GetRandomString(11) : txtIdentOptionalTwo.Text;
            
            btnIdentGenerate_Click(null, null);
        }
    }
}
