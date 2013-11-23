using EventStore.ClientAPI;
using Ninject.Modules;

namespace TwitterQuiz.EventStore
{
    public class EventStoreNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventStoreConnection>()
                .ToMethod(x =>
                    {
                        var connection = EventStoreConnectionProvider.EventStore;
                        connection.Connect();
                        return connection;
                    })
                .InSingletonScope();
        }
    }
}