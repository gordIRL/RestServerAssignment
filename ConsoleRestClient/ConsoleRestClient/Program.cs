using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleRestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");
            RunAsync().Wait();           
         
            Console.ReadKey();
        }



        static async Task RunAsync()
        {
            // VIDEO Client_1(15:39)
            using (var client = new HttpClient())
            {
                // go get the data
                client.BaseAddress = new Uri("http://localhost:49661/");
                client.DefaultRequestHeaders.Accept.Clear();

                // tell it that we want JSON back
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine("\n\nPOST");

                HttpResponseMessage response; 

                UserEntry newUserEntry = new UserEntry
                {
                    Category = "Bungi Jumping",
                    Notes = "Never!",
                    StartDate = DateTime.Parse("2018-10,25"),
                    EndDate = DateTime.Parse("2019/9/25")
                };

                response = await client.PostAsJsonAsync("api/UserEntry", newUserEntry);
                if (response.IsSuccessStatusCode)
                {
                    Uri userEntryUrl = response.Headers.Location;
                    Console.WriteLine(userEntryUrl);

                    // PUT
                    newUserEntry.Notes = "PUT is working";
                    response = await client.PutAsJsonAsync(userEntryUrl, newUserEntry);
                    Console.WriteLine();


                    // DELETE
                    response = await client.DeleteAsync(userEntryUrl);
                    Console.WriteLine();
                }


                Console.WriteLine("\n\nGET");
                response = await client.GetAsync("api/UserEntry/26");

                if (response.IsSuccessStatusCode)
                {
                    //string myTest = await response.Content.ReadAsStringAsync();
                    //Console.Write("My string is : {0}", myTest);

                    UserEntry userEntry = await response.Content.ReadAsAsync<UserEntry>();
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",
                        userEntry.Id, userEntry.Category, userEntry.Notes, userEntry.StartDate, userEntry.EndDate);                    
                }
                else
                {
                    Console.Write("No data to return");
                }


                Console.WriteLine("\n\nGET all user entries");
                response = await client.GetAsync("api/UserEntry");

                if (response.IsSuccessStatusCode)
                {
                    List<UserEntry> userEntryList = await response.Content.ReadAsAsync<List<UserEntry>>();

                    foreach (var userEntry in userEntryList)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",
                        userEntry.Id, userEntry.Category, userEntry.Notes, userEntry.StartDate, userEntry.EndDate);
                    }                    
                }
                else
                {
                    Console.Write("No data to return");
                }
            }
        }
    }
}
