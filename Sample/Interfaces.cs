namespace Lokad
{
    public interface IIdentity { }

    public interface ICommand { }
    public interface IEvent { }


    public interface IUniverseCommand { }
    public interface IUniverseEvent<out TIdentity> : IEvent
        where TIdentity : IIdentity
    {
    }
}
