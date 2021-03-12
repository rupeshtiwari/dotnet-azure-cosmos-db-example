using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

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

            await this.client.CreateDocumentCollectionIfNotExistsAsync (UriFactory.CreateDatabaseUri ("customers"), new DocumentCollection {
                Id = "users", PartitionKey = new PartitionKeyDefinition () { Paths = new System.Collections.ObjectModel.Collection<string> () { "/userId" } }
            });

            Console.WriteLine ("Database and collection creation/validation is complete");

            await this.CreateUserDocumentIfNotExists ("customers", "users", new UserData ().nelapin);
            await this.CreateUserDocumentIfNotExists ("customers", "users", new UserData ().yanhe);
            await this.ReadUserDocument ("customers", "users", new UserData ().yanhe);
        }

        private async Task CreateUserDocumentIfNotExists (string databaseName, string collectionName, User user) {
            try {
                await this.client.ReadDocumentAsync (UriFactory.CreateDocumentUri (databaseName, collectionName, user.Id), new RequestOptions { PartitionKey = new PartitionKey (user.UserId) });

                this.WriteToConsoleAndPromptToContinue ("User {0} already exists in the database", user.Id);
            } catch (DocumentClientException de) {
                if (de.StatusCode == HttpStatusCode.NotFound) {
                    await this.client.CreateDocumentAsync (UriFactory.CreateDocumentCollectionUri (databaseName, collectionName), user);
                    this.WriteToConsoleAndPromptToContinue ("Created User {0}", user.Id);
                } else {
                    throw;
                }
            }
        }

        private void WriteToConsoleAndPromptToContinue (string format, params object[] args) {
            Console.WriteLine (format, args);
            Console.WriteLine ("Press any key to continue ...");
            Console.ReadKey ();
        }

        private async Task ReadUserDocument (string databaseName, string collectionName, User user) {
            try {
                var userResource = await this.client.ReadDocumentAsync (UriFactory.CreateDocumentUri (databaseName, collectionName, user.Id), new RequestOptions { PartitionKey = new PartitionKey (user.UserId) });
                var userFromDb = userResource.Resource;
                this.WriteToConsoleAndPromptToContinue ("Read user {0}", user.Id);
                this.WriteToConsoleAndPromptToContinue ("Read user {0}", Newtonsoft.Json.JsonConvert.SerializeObject (userFromDb, Formatting.Indented));
            } catch (DocumentClientException de) {
                if (de.StatusCode == HttpStatusCode.NotFound) {
                    this.WriteToConsoleAndPromptToContinue ("User {0} not read", user.Id);
                } else {
                    throw;
                }
            }
        }
    }
}