using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator
{
    public class IdentityDocumentModel : DocumentBase
    {
        public IdentityDocumentModel(CountryCode countryOfIssue, string surName, string givenNames, string documentNum, string dob, string expDate,
            string optionalOne, string optionalTwo, string placeOfBirth, CountryCode nationality, SexCode sex, string docType)
        {
            _countryOfIssue = countryOfIssue;
            _surName = surName;
            _givenNames = givenNames;
            _documentNum = documentNum;
            _dob = dob;
            _expDate = expDate;
            _optionalOne = optionalOne;
            _optionalTwo = optionalTwo;
            _placeOfBirth = placeOfBirth;
            _nationality = nationality;
            _sex = sex;
            _docType = docType;
        }

        string[] _mrz;
        string _docType = string.Empty;
        CountryCode _countryOfIssue;
        string _surName = string.Empty;
        string _givenNames = string.Empty;
        
        string _documentNum= string.Empty;
        string _dob= string.Empty;
        string _expDate= string.Empty;
        string _optionalOne= string.Empty;
        string _optionalTwo = string.Empty;
        string _placeOfBirth= string.Empty; 
        CountryCode _nationality;
        SexCode _sex;

        public string[] MRZ
        {
            get { return _mrz; }
            set { _mrz = value; }
        }
        public string DocType
        {
            get { return _docType; }
            set { _docType = value; }
        }
        public CountryCode CountryOfIssue
        {
            get { return _countryOfIssue; }
            set { _countryOfIssue = value; }
        }
        public string SurName
        {
            get { return _surName; }
            set { _surName = value; }
        }
        public string GivenNames
        {
            get { return _givenNames; }
            set { _givenNames = value; }
        }
        public string DocumentNum
        {
            get { return _documentNum; }
            set { _documentNum = value; }
        }
        public string Dob
        {
            get { return _dob; }
            set { _dob = value; }
        }
        public string ExpDate
        {
            get { return _expDate; }
            set { _expDate = value; }
        }
        public string OptionalOne
        {
            get { return _optionalOne; }
            set { _optionalOne = value; }
        }
        public string OptionalTwo
        {
            get { return _optionalTwo; }
            set { _optionalTwo = value; }
        }

        public CountryCode Nationality
        {
            get { return _nationality; }
            set { _nationality = value; }
        }
        public SexCode Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
    }
}
