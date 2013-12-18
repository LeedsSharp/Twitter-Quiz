using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;

namespace TwitterQuiz.Ninject
{
    public class RavenDbNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>()
           .ToMethod(context => RavenSessionProvider.DocumentStore)
           .InSingletonScope();

            Bind<IDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }
    }
}