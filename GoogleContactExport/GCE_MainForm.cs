using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;


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
                new[] { "email", "profile", "https://www.googleapis.com/auth/contacts.readonly" },
                "me",
                CancellationToken.None).Result;

            // Create the service.
            peopleService = new PeopleService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleContactExport",
            });

            lblStatus.Text = "Authenticated!";

            btnGetContacts.Enabled = true;
            btnRemoveAuth.Enabled = true;
            btnAuth.Enabled = false;
            
            //PeopleResource.GetRequest peopleRequest = peopleService.People.Get("people/me");
            //peopleRequest.RequestMaskIncludeField = "person.emailAddresses";
            //Person profile = peopleRequest.Execute();
            //lblStatus.Text = "Authenticated as: " + profile.EmailAddresses[0].Value;
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
                        if (numb.Type == "work" && string.IsNullOrEmpty(args.businessPhone))
                            args.businessPhone = numb.Value;
                        else if (numb.Type == "work" && !string.IsNullOrEmpty(args.businessPhone))
                            args.business2Phone = numb.Value;
                        else if (numb.Type == "home" && string.IsNullOrEmpty(args.privatePhone))
                            args.privatePhone = numb.Value;
                        else if (numb.Type == "mobile" && string.IsNullOrEmpty(args.mobilePhone))
                            args.mobilePhone = numb.Value;
                        else if (numb.Type == "homeFax" && string.IsNullOrEmpty(args.privateFax))
                            args.privateFax = numb.Value;
                        else if (numb.Type == "workFax" && string.IsNullOrEmpty(args.businessFax))
                            args.businessFax = numb.Value;
                        else if (numb.Type == "pager" && string.IsNullOrEmpty(args.pager))
                            args.pager = numb.Value;
                        else
                            args.notes += "Other Phone Numbers: " + numb.Type + ": " + numb.Value + Environment.NewLine;
                    }
                }

                if (person.EmailAddresses != null)
                {
                    foreach (var mail in person.EmailAddresses)
                    {
                        if (mail.Type == "home")
                            args.privateEmail = mail.Value;
                        else if (mail.Type == "work" && string.IsNullOrEmpty(args.businessEmail))
                            args.businessEmail = mail.Value;
                        else if (mail.Type == "work" && !string.IsNullOrEmpty(args.businessEmail) && string.IsNullOrEmpty(args.business2Email))
                            args.business2Email = mail.Value;
                        else if (mail.Type == "other" && string.IsNullOrEmpty(args.businessEmail))
                            args.businessEmail = mail.Value;
                        else if (mail.Type == "other" && string.IsNullOrEmpty(args.business2Email))
                            args.business2Email = mail.Value;
                        else if (mail.Type == "other" && string.IsNullOrEmpty(args.privateEmail))
                            args.privateEmail = mail.Value;
                        else
                            args.notes += "Other E-Mail Addresses: " + mail.Type + ": " + mail.Value + Environment.NewLine;
                    }
                }

                if (person.Biographies != null)
                {
                    if (!string.IsNullOrEmpty(args.notes))
                        args.notes += Environment.NewLine + person.Biographies[0].Value;
                    else
                        args.notes = person.Biographies[0].Value;
                }

                if (person.Birthdays != null)
                {
                    args.birthday = new DateTime(person.Birthdays[0].Date.Year.Value, person.Birthdays[0].Date.Month.Value, person.Birthdays[0].Date.Day.Value);
                }

                if (person.Genders != null)
                {
                    args.gender = person.Genders[0].Value;
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
