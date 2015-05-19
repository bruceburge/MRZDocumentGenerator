using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator
{
    public static class CheckDigitCalculator
    {
        public static string GeneratePassportMRZ(PassportModel passport)
        {

            passport.PersonalNum = passport.PersonalNum.Replace(' ', '<').PadRight(14, '<');

            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("passport", mrzCheckDigitConvert(passport.PassportNum).ToString());
            result.Add("dob", mrzCheckDigitConvert(passport.Dob).ToString());
            result.Add("expDate", mrzCheckDigitConvert(passport.ExpDate).ToString());
            result.Add("personalNum", mrzCheckDigitConvert(passport.PersonalNum).ToString());
            string final = passport.PassportNum + result["passport"].ToString() + passport.Dob + result["dob"].ToString() + passport.ExpDate + result["expDate"].ToString() + passport.PersonalNum + result["personalNum"].ToString();
            result.Add("final", mrzCheckDigitConvert(final).ToString());

            string MRZ1;
            string name = passport.SurName.ToUpper() + "<<" + passport.GivenNames.Replace(' ', '<').Replace('-', '<').ToUpper();
            if (name.Length > 39)
            {
                name = name.Substring(0, 39);
            }
            MRZ1 = passport.DocType.PadRight(2,'<').ToUpper()+passport.CountryOfIssue;
            MRZ1 += name;
	        MRZ1 = MRZ1.PadRight(44,'<');
              
            string total = passport.PassportNum + result["passport"].ToString() + passport.Nationality.ToString() + passport.Dob + result["dob"].ToString() + 
                Convert.ToChar(Convert.ChangeType(passport.Sex,passport.Sex.GetTypeCode()))
                + passport.ExpDate + result["expDate"].ToString() + passport.PersonalNum + result["personalNum"] + result["final"].ToString();
            //total.Dump("Total");
            result.Add("mrz", total);

            return MRZ1+Environment.NewLine+total;
        }
        
        
        public static string GenerateIdentityCardMRZ(IdentityDocumentModel document)
        {
            string lineOne = string.Empty;
            string lineTwo = string.Empty;
            string lineThree = string.Empty;


            lineOne = document.DocType.PadRight(2, '<');
            lineOne += document.CountryOfIssue;
            lineOne += document.DocumentNum;
            lineOne += mrzCheckDigitConvert(document.DocumentNum);
            lineOne += document.OptionalOne;
            lineOne = lineOne.PadRight(30, '<').Substring(0, 30);

            lineTwo = document.Dob;
            lineTwo += mrzCheckDigitConvert(document.Dob);
            lineTwo += Convert.ToChar(Convert.ChangeType(document.Sex, document.Sex.GetTypeCode()));
            lineTwo += document.ExpDate;
            lineTwo += mrzCheckDigitConvert(document.ExpDate);
            lineTwo += document.Nationality;
            lineTwo += document.OptionalTwo;
            lineTwo = lineTwo.PadRight(30, '<').Substring(0, 29);
            lineTwo += mrzCheckDigitConvert(lineOne.Substring(5,25)+lineTwo.Substring(0,7)+lineTwo.Substring(8,7)+lineTwo.Substring(18,10));

            lineThree = (document.SurName + "<<" + document.GivenNames).PadRight(30, '<').Substring(0, 30); ;
            
            return lineOne+Environment.NewLine+lineTwo+Environment.NewLine+lineThree;
        }


        private static int mrzCheckDigitConvert(string phrase)
        {
            int result = 0;
            int count = 0;
            int charValue = 0;

            Dictionary<char, int> mrzCheckDigitLookup = new Dictionary<char, int>();

            mrzCheckDigitLookup.Add('<', 0);
            mrzCheckDigitLookup.Add('A', 10);
            mrzCheckDigitLookup.Add('B', 11);
            mrzCheckDigitLookup.Add('C', 12);
            mrzCheckDigitLookup.Add('D', 13);
            mrzCheckDigitLookup.Add('E', 14);
            mrzCheckDigitLookup.Add('F', 15);
            mrzCheckDigitLookup.Add('G', 16);
            mrzCheckDigitLookup.Add('H', 17);
            mrzCheckDigitLookup.Add('I', 18);
            mrzCheckDigitLookup.Add('J', 19);
            mrzCheckDigitLookup.Add('K', 20);
            mrzCheckDigitLookup.Add('L', 21);
            mrzCheckDigitLookup.Add('M', 22);
            mrzCheckDigitLookup.Add('N', 23);
            mrzCheckDigitLookup.Add('O', 24);
            mrzCheckDigitLookup.Add('P', 25);
            mrzCheckDigitLookup.Add('Q', 26);
            mrzCheckDigitLookup.Add('R', 27);
            mrzCheckDigitLookup.Add('S', 28);
            mrzCheckDigitLookup.Add('T', 29);
            mrzCheckDigitLookup.Add('U', 30);
            mrzCheckDigitLookup.Add('V', 31);
            mrzCheckDigitLookup.Add('W', 32);
            mrzCheckDigitLookup.Add('X', 33);
            mrzCheckDigitLookup.Add('Y', 34);
            mrzCheckDigitLookup.Add('Z', 35);

            foreach (var letter in phrase.Replace(' ','<').ToUpper().ToCharArray())
            {
                count++;

                if (Char.IsNumber(letter))
                {
                    charValue = (int)char.GetNumericValue(letter);
                }
                else if (mrzCheckDigitLookup.ContainsKey(letter))
                {
                    charValue = mrzCheckDigitLookup[letter];
                }
                else
                {
                    var ex = new Exception("Unknown character in MRZ :" + letter);
                    throw ex;
                }

                var weightedcharValue = WeightingMultiplier(count, charValue);
                result += weightedcharValue;
                var tmp = " " + letter + "[" + charValue + "] * " + WeightingMultiplier(count, 1) + " = " + weightedcharValue + " += " + result;
                if (count > 2)
                {
                    count = 0;
                }
                //tmp.Dump();
            }

            var remainder = result % 10;
            //(result+" % 10 = "+remainder+Environment.NewLine).Dump();

            return remainder;
        }

        private static int WeightingMultiplier(int count, int value)
        {
            switch (count)
            {
                case 1:
                    value *= 7;
                    break;
                case 2:
                    value *= 3;
                    break;
                case 3:
                    value *= 1;
                    break;
            }

            return value;
        }
    }
}
