using System.Configuration;
using Raven.Client.Document;

namespace TwitterQuiz
{
    public class RavenSessionProvider
    {
        private static DocumentStore _documentStore;

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
    }
}