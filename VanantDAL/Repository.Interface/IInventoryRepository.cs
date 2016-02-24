using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanantDAL.Repository.Interface;
using VanantModel;

namespace VanantDAL.Repository.Interface
{
    public interface IInventoryRepository
    {
        void Add(InventoryItem inventoryItem);

        InventoryItem GetByLabel(string label);

        int DeleteByLabel(string label);

        List<InventoryItem> GetByLessExpiration(DateTime now);

        void Reset();
    }
}
