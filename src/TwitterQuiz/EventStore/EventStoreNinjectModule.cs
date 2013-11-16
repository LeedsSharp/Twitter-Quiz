using EventStore.ClientAPI;
using Ninject.Modules;

namespace TwitterQuiz.EventStore
{
    public class EventStoreNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventStoreConnection>()
                .ToMethod(x => EventStoreConnectionProvider.EventStore)
                .InSingletonScope();
        }
    }
}