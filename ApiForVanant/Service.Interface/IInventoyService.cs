using VanantModel;

namespace ApiForVanant.Service.Interface
{
    public delegate void NotifyEventHandler(string message);

    public interface IInventoyService
    {
        event NotifyEventHandler Notify;

        void Add(InventoryItem invetoryItem);

        InventoryItem Take(string label);

        void Reset();
    }
}
