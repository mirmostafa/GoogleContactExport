namespace GoogleContactExport
{
    public class ContactEventArgs
    {
        //Name
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        
        //Adresses
        public string privateStreet { get; set; }
        public string privateCity { get; set; }
        public string privateRegion { get; set; }
        public string privatePostcode { get; set; }
        public string privateCountry { get; set; }
        public string businessStreet { get; set; }
        public string businessCity { get; set; }
        public string businessRegion { get; set; }
        public string businessPostcode { get; set; }
        public string businessCountry { get; set; }

        //Phone Numbers
        public string privatePhone { get; set; }
        public string mobilePhone { get; set; }
        public string privateFax { get; set; }
        public string businessPhone { get; set; }
        public string business2Phone { get; set; }
        public string businessFax { get; set; }
        public string pager { get; set; }

        //E-Mail Adresses
        public string privateEmail { get; set; }
        public string businessEmail { get; set; }
        public string business2Email { get; set; }

        //Other
        public string notes { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public string company { get; set; }
        public string jobTitle { get; set; }



    }
}
