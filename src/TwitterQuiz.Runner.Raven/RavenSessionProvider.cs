using System.Configuration;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace TwitterQuiz.Runner.Raven
{
    public class RavenSessionProvider
    {
        private static DocumentStore _documentStore;

        public bool Embeddable { get; set; }
        public bool SessionInitialized { get; set; }

        public static DocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static DocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
            {
                Url = ConfigurationManager.AppSettings["RAVEN_URI"],
                DefaultDatabase = ConfigurationManager.AppSettings["RAVEN_Database"],
                ApiKey = ConfigurationManager.AppSettings["RAVEN_APIKEY"]
            };
            store.Initialize();

            return store;
        }

        public static DocumentStore EmbeddableDocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateEmbeddableDocumentStore())); }
        }

        private static DocumentStore CreateEmbeddableDocumentStore()
        {
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8082);
            var store = new EmbeddableDocumentStore
            {
                DataDirectory = "App_Data",
                UseEmbeddedHttpServer = true,
                Configuration = { Port = 8080 }
            };
            store.Initialize();
            return store;
        }
    }
}