using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace GoogleContactExport
{
    public partial class GCE_MainForm : Form
    {
        PeopleService peopleService;

        public GCE_MainForm()
        {
            InitializeComponent();
        }

        private void GCE_MainForm_Shown(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Google.Apis.Auth", "Google.Apis.Auth.OAuth2.Responses.TokenResponse-me")))
            {
                btnRemoveAuth.Enabled = true;
                btnGetContacts.Enabled = true;
                btnAuth.Enabled = false;

                btnAuth_Click(this, e);
            }
            else
                lblStatus.Text = "Ready to authenticate...";
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Authenticating...";

            // Create OAuth credential.
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "425537419269-36uofj9uv00ru9qp9653l3u8o2vjvqhn.apps.googleusercontent.com",
                    ClientSecret = "9ur93YzU8rLlAETexFE4KvuW"
                },
                new[] { "profile", "https://www.googleapis.com/auth/contacts.readonly" },
                "me",
                CancellationToken.None).Result;

            // Create the service.
            peopleService = new PeopleService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleContactExport",
            });

            lblStatus.Text = "Authenticated as: ";

            btnGetContacts.Enabled = true;
            btnRemoveAuth.Enabled = true;
            btnAuth.Enabled = false;

            PeopleResource.GetRequest peopleRequest = peopleService.People.Get("people/me");
            peopleRequest.RequestMaskIncludeField = "person.emailAddresses";
            lblStatus.Text = "Authenticated as: " + peopleRequest.Execute();
        }

        private void btnGetContacts_Click(object sender, EventArgs e)
        {
            PeopleResource.ConnectionsResource.ListRequest peopleRequest = peopleService.People.Connections.List("people/me");
            peopleRequest.RequestMaskIncludeField = "person.addresses,person.biographies,person.birthdays,person.emailAddresses," +
                                                    "person.genders,person.names,person.organizations,person.phoneNumbers";
            peopleRequest.PageSize = 2000;
            ListConnectionsResponse connectionsResponse = peopleRequest.Execute();
            IList<Person> connections = connectionsResponse.Connections;

            pbrContacts.Maximum = connections.Count;

            int contacts = 0;
            foreach (Person person in connections)
            {
                ContactEventArgs args = new ContactEventArgs();

                if (person.Names != null)
                {
                    args.firstName = person.Names[0].GivenName;
                    args.lastName = person.Names[0].FamilyName;
                    args.middleName = person.Names[0].MiddleName;
                }

                if (person.Addresses != null)
                {
                    foreach (var addr in person.Addresses)
                    {
                        if (addr.Type == "work")
                        {
                            args.businessStreet = addr.StreetAddress;
                            args.businessCity = addr.City;
                            args.businessRegion = addr.Region;
                            args.businessPostcode = addr.PostalCode;
                            args.businessCountry = addr.Country;
                        }
                        else
                        {
                            args.privateStreet = addr.StreetAddress;
                            args.privateCity = addr.City;
                            args.privateRegion = addr.Region;
                            args.privatePostcode = addr.PostalCode;
                            args.privateCountry = addr.Country;
                        }
                    }
                }

                if (person.PhoneNumbers != null)
                {
                    foreach (var numb in person.PhoneNumbers)
                    {
                        if (numb.Type == "work" && args.businessPhone == string.Empty)
                            args.businessPhone = numb.Value;
                        else if (numb.Type == "work" && args.businessPhone != string.Empty)
                            args.business2Phone = numb.Value;
                        else if (numb.Type == "home")
                            args.privatePhone = numb.Value;
                        else if (numb.Type == "mobile")
                            args.mobilePhone = numb.Value;
                        else if (numb.Type == "homeFax")
                            args.privateFax = numb.Value;
                        else if (numb.Type == "workFax")
                            args.businessFax = numb.Value;
                        else if (numb.Type == "pager")
                            args.pager = numb.Value;
                    }
                }

                if (person.EmailAddresses != null)
                {
                    foreach (var mail in person.EmailAddresses)
                    {
                        if (mail.Type == "home")
                            args.privateEmail = mail.Value;
                        else if (mail.Type == "work" && args.businessEmail == string.Empty)
                            args.businessEmail = mail.Value;
                        else if (mail.Type == "work" && args.businessEmail != string.Empty)
                            args.business2Email = mail.Value;
                        else if (mail.Type == "other" && args.businessEmail == string.Empty)
                            args.businessEmail = mail.Value;
                        else if (mail.Type == "other" && args.business2Email == string.Empty)
                            args.business2Email = mail.Value;
                        else if (mail.Type == "other" && args.privateEmail == string.Empty)
                            args.privateEmail = mail.Value;
                        else
                            args.notes = "Other E-Mail Addresses: " + mail.Value;
                    }
                }

                if (person.Biographies != null)
                {
                    if (args.notes != string.Empty)
                        args.notes = args.notes + Environment.NewLine + Environment.NewLine + person.Biographies[0].Value;
                    else
                        args.notes = person.Biographies[0].Value;
                }

                if (person.Birthdays != null)
                        args.birthday = person.Birthdays[0].Text;

                if (person.Genders != null)
                {
                    if (person.Genders[0].Value == "male")
                        args.gender = "Männlich";
                    else if (person.Genders[0].Value == "female")
                        args.gender = "Weiblich";
                    else
                        args.gender = "Sonstige";
                }

                if (person.Organizations != null)
                {
                    args.company = person.Organizations[0].Name;
                    args.jobTitle = person.Organizations[0].Title;
                }

                OnContactReady(args);
                contacts++;
                pbrContacts.PerformStep();
            }
            MessageBox.Show("Successfully imported " + contacts + " contacts!");
        }

        private void btnRemoveAuth_Click(object sender, EventArgs e)
        {
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Google.Apis.Auth", "Google.Apis.Auth.OAuth2.Responses.TokenResponse-me"));
            btnAuth.Enabled = true;
            btnRemoveAuth.Enabled = false;
            lblStatus.Text = "Ready to authenticate...";
        }

        protected virtual void OnContactReady(ContactEventArgs e)
        {
            ContactReady?.Invoke(this, e);
        }

        public event EventHandler<ContactEventArgs> ContactReady;

        private void GCE_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnGetContacts.Enabled = false;
            pbrContacts.Value = 0;
        }
    }
}
