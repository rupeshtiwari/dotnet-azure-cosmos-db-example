using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace azure_cosmos_db_example {
    class Program {

        private DocumentClient client;

        static void Main (string[] args) {
            try {
                Program p = new Program ();
                p.InitializeDB ().Wait ();
            } catch (DocumentClientException de) {
                Exception baseException = de.GetBaseException ();
                Console.WriteLine ("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            } catch (Exception e) {
                Exception baseException = e.GetBaseException ();
                Console.WriteLine ("Error: {0}, Message: {1}", e.Message, baseException.Message);
            } finally {
                Console.WriteLine ("End of demo, press any key to exit.");
                Console.ReadKey ();
            }
        }

        private async Task InitializeDB () {
            this.client = new DocumentClient (new Uri (ConfigurationManager.AppSettings["accountEndpoint"]), ConfigurationManager.AppSettings["accountKey"]);

            await this.client.CreateDatabaseIfNotExistsAsync (new Database { Id = "customers" });

            await this.client.CreateDocumentCollectionIfNotExistsAsync (UriFactory.CreateDatabaseUri ("customers"), new DocumentCollection { Id = "users" });

            Console.WriteLine ("Database and collection creation/validation is complete");
        }
    }
}