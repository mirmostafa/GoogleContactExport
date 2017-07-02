using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using System.Diagnostics;

namespace GoogleContactExport
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _mf.ContactReady += AddContact;
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            int majorVersion = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName).ProductMajorPart;

            if (majorVersion == 15)
                return new Ribbon();
            else
                return new Ribbon2();
        }

        private static readonly GCE_MainForm _mf = new GCE_MainForm();

        public static GCE_MainForm mfInstance
        {
            get { return _mf; }
        }

        public void AddContact(object sender, ContactEventArgs e)
        {
            Outlook.ContactItem newContact = (Outlook.ContactItem)this.Application.CreateItem(Outlook.OlItemType.olContactItem);

            try
            {
                newContact.FirstName = e.firstName;
                newContact.LastName = e.lastName;
                newContact.MiddleName = e.middleName;

                newContact.HomeAddressStreet = e.privateStreet;
                newContact.HomeAddressCity = e.privateCity;
                newContact.HomeAddressState = e.privateRegion;
                newContact.HomeAddressPostalCode = e.privatePostcode;
                newContact.HomeAddressCountry = e.privateCountry;
                newContact.BusinessAddressStreet = e.businessStreet;
                newContact.BusinessAddressCity = e.businessCity;
                newContact.BusinessAddressState = e.businessRegion;
                newContact.BusinessAddressPostalCode = e.businessPostcode;
                newContact.BusinessAddressCountry = e.businessCountry;

                newContact.HomeTelephoneNumber = e.privatePhone;
                newContact.MobileTelephoneNumber = e.mobilePhone;
                newContact.HomeFaxNumber = e.privateFax;
                newContact.BusinessTelephoneNumber = e.businessPhone;
                newContact.Business2TelephoneNumber = e.business2Phone;
                newContact.BusinessFaxNumber = e.businessFax;
                newContact.PagerNumber = e.pager;

                newContact.Email1Address = e.privateEmail;
                newContact.Email2Address = e.businessEmail;
                newContact.Email3Address = e.business2Email;

                newContact.Body = e.notes;
                //newContact.Birthday = e.birthday;
                //newContact.Gender = e.gender;
                newContact.Companies = e.company;
                newContact.JobTitle = e.jobTitle;

                newContact.Save();
            }
            catch
            {
                MessageBox.Show("The new contact was not saved.");
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Hinweis: Outlook löst dieses Ereignis nicht mehr aus. Wenn Code vorhanden ist, der 
            //    muss ausgeführt werden, wenn Outlook heruntergefahren wird. Weitere Informationen finden Sie unter https://go.microsoft.com/fwlink/?LinkId=506785.
        }

        #region Von VSTO generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
