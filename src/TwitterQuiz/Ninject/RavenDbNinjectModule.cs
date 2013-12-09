using Ninject.Modules;
using Raven.Client;

namespace TwitterQuiz.Ninject
{
    public class RavenDbNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentSession>()
                .ToMethod(x =>
                {
                    var store = RavenSessionProvider.DocumentStore;
                    return store.OpenSession();
                })
                .InSingletonScope();
        }
    }
}