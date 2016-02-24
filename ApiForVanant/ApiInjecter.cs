using System.Reflection;
using ApiForVanant.Service.Interface;
using Ninject;

namespace ApiForVanant
{
    /// <summary>
    /// Use this class to get the services ready to run by default.
    /// </summary>
    public class ApiInjecter
    {
        private StandardKernel NinjectKernel()
        { 
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        /// <summary>
        /// Gets an inventory service
        /// </summary>
        public IInventoyService GetInventoryService()
        {
            return NinjectKernel().Get<IInventoyService>();
        }
    }
}
