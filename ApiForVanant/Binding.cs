using ApiForVanant.Service;
using ApiForVanant.Service.Interface;
using Ninject.Modules;
using VanantDAL.Repository;
using VanantDAL.Repository.Interface;

namespace ApiForVanant
{
    /// <summary>
    /// This is needed to make Ninject know which class has to be injected depending on the interface used
    /// </summary>
    public class Binding : NinjectModule
    {
        public override void Load()
        {
            //Repositories
            Bind<IInventoryRepository>().To<InventoryRepository>();

            //Services
            Bind<IInventoyService>().To<InventoyService>();
        }
    }
}
