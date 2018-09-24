using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
 

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        String localHostAddress = "http://localhost:49661/";

        public Form1()
        {
            InitializeComponent();
                        
            comboCategory.DataSource = Enum.GetValues(typeof(ActivitiesEnum));

            timePicker_start.Format = DateTimePickerFormat.Time;
            timePicker_start.ShowUpDown = true;
            datePicker_start.Format = DateTimePickerFormat.Short;
            datePicker_start.ShowUpDown = true;

            timePicker_end.Format = DateTimePickerFormat.Time;
            timePicker_end.ShowUpDown = true;
            datePicker_end.Format = DateTimePickerFormat.Short;
            datePicker_end.ShowUpDown = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBoxDisplay.Items.Clear();
        }

        private void btnClearControls_Click(object sender, EventArgs e)
        {
            txtNotes.Clear();
            txtIndividual.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btnGetAll_Click(object sender, EventArgs e)
        {
            listBoxDisplay.Items.Clear();
            
            using (var client = new HttpClient())
            {
                // go get the data
                //client.BaseAddress = new Uri("http://localhost:49661/");
                client.BaseAddress = new Uri(localHostAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                // tell it that we want JSON back
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;               
                response = await client.GetAsync("api/UserEntry");
                                
                if (response.IsSuccessStatusCode)
                {
                    List<UserEntry> userEntryList = await response.Content.ReadAsAsync<List<UserEntry>>();

                    foreach (var userEntry in userEntryList)
                    {
                        string tempUserEntry = string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                            userEntry.Id, userEntry.Category, userEntry.Notes, userEntry.StartDate, userEntry.EndDate);

                        listBoxDisplay.Items.Add(tempUserEntry);
                    }
                }
                else
                {
                    listBoxDisplay.Items.Clear();
                    listBoxDisplay.Items.Add("No data to return");
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            listBoxDisplay.Items.Clear();

            try
            {
                using (var client = new HttpClient())
                {
                    // go get the data
                    client.BaseAddress = new Uri(localHostAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;

                    int.TryParse(txtIndividual.Text, out int id);
                    if(id > 0)
                    {
                        response = await client.DeleteAsync("api/UserEntry/" + id.ToString());
                        MessageBox.Show("Record deleted");
                    }
                    else
                    {
                        MessageBox.Show("Invalid - no record deleted");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Invalid - no record deleted");
            }
            txtIndividual.Clear();
        }

        private async void btnGetIndividual_Click(object sender, EventArgs e)
        {
            listBoxDisplay.Items.Clear();

            int.TryParse(txtIndividual.Text, out int id);

            using (var client = new HttpClient())
            {
                // go get the data
                client.BaseAddress = new Uri(localHostAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                // tell it that we want JSON back
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                response = await client.GetAsync("api/UserEntry/" + id.ToString());

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        UserEntry userEntry = await response.Content.ReadAsAsync<UserEntry>();
                        
                        string tempUserEntry = string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                            userEntry.Id, userEntry.Category, userEntry.Notes, userEntry.StartDate, userEntry.EndDate);

                        listBoxDisplay.Items.Add(tempUserEntry);
                    }
                    catch
                    {                       
                        MessageBox.Show("Non existent Id");
                    }                    
                }
                else
                {
                    listBoxDisplay.Items.Clear();
                    listBoxDisplay.Items.Add("No data to return");
                }
            }
        }

        private async void btnAddEntry_Click(object sender, EventArgs e)
        {
            string category = comboCategory.Text;
            string notes = txtNotes.Text;
            DateTime startDateTime = datePicker_start.Value.Date +
                    timePicker_start.Value.TimeOfDay;

            DateTime endDateTime = datePicker_end.Value.Date +
                    timePicker_end.Value.TimeOfDay;

            listBoxDisplay.ClearSelected();
            listBoxDisplay.Items.Add("Category: " + category);
            listBoxDisplay.Items.Add("Start: " + startDateTime.ToString());
            listBoxDisplay.Items.Add("Finish: " + endDateTime.ToString());
            listBoxDisplay.Items.Add("Notes:");
            listBoxDisplay.Items.Add(notes);
                       
            using (var client = new HttpClient())
            {
                // go get the data
                client.BaseAddress = new Uri(localHostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                UserEntry newUserEntry = new UserEntry
                {
                    Category = category,
                    Notes = notes,
                    StartDate = startDateTime,
                    EndDate = endDateTime
                };

                response = await client.PostAsJsonAsync("api/UserEntry", newUserEntry);

                if (response.IsSuccessStatusCode)
                {
                    Uri userEntryUrl = response.Headers.Location;
                    MessageBox.Show("Url of newly created record: \n" + userEntryUrl.ToString());

                    try
                    {
                        response = await client.GetAsync(userEntryUrl);
                        UserEntry userEntry = await response.Content.ReadAsAsync<UserEntry>();
                        MessageBox.Show("Id of new record: " + userEntry.Id.ToString());                        
                    }
                    catch
                    {
                        MessageBox.Show("Error - cannot retrieve Id");
                    }
                }
            }
            txtNotes.Clear();
            txtIndividual.Clear();
        }


        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string category = comboCategory.Text;
            string notes = txtNotes.Text;
            DateTime startDateTime = datePicker_start.Value.Date +
                    timePicker_start.Value.TimeOfDay;

            DateTime endDateTime = datePicker_end.Value.Date +
                    timePicker_end.Value.TimeOfDay;

            listBoxDisplay.ClearSelected();
            listBoxDisplay.Items.Add("Category: " + category);
            listBoxDisplay.Items.Add("Start: " + startDateTime.ToString());
            listBoxDisplay.Items.Add("Finish: " + endDateTime.ToString());
            listBoxDisplay.Items.Add("Notes:");
            listBoxDisplay.Items.Add(notes);

            UserEntry newUserEntry = new UserEntry
            {
                Category = category,
                Notes = notes,
                StartDate = startDateTime,
                EndDate = endDateTime
            };

            int.TryParse(txtIndividual.Text, out int id);

            
            using (var client = new HttpClient())
            {
                // go get the data
                client.BaseAddress = new Uri(localHostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                //PUT               
                response = await client.PutAsJsonAsync("api/UserEntry/" + id.ToString(), newUserEntry);
            }
            txtNotes.Clear();
            txtIndividual.Clear();
        }
    }
}
