using EventStore.ClientAPI;
using Ninject.Modules;
using TwitterQuiz.EventStore;

namespace TwitterQuiz.Ninject
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