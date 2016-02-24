using System;
using System.Linq;
using System.Collections.Generic;
using VanantDAL.Repository.Interface;
using VanantModel;

namespace VanantDAL.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        /// <summary>
        /// Static dictionary:
        /// This is a bad usually a practice. I am doing it because it is allowed in the exercise and to simplify it
        /// </summary>
        private static Dictionary<string, InventoryItem> dictionary = new Dictionary<string, InventoryItem>();

        #region Public

        public void Add(InventoryItem inventoryItem)
        {
            if (!InventoryRepository.dictionary.ContainsKey(inventoryItem.Label))
            {
                InventoryRepository.dictionary.Add(inventoryItem.Label, inventoryItem);
            } 
            // If it already exist I am assuming that nothing has to be done
        }

        public int DeleteByLabel(string label)
        {
            if (InventoryRepository.dictionary.ContainsKey(label))
            {
                InventoryRepository.dictionary.Remove(label);
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public InventoryItem GetByLabel(string label)
        {
            if (InventoryRepository.dictionary.ContainsKey(label))
            {                
                return InventoryRepository.dictionary[label]; 
            }
            else
            {
                return null;
            }
        }

        public List<InventoryItem> GetByLessExpiration(DateTime now)
        {
            List<InventoryItem> list = InventoryRepository.dictionary.Values.Where(i => i.ExpirationDate <= now).ToList();

            return list;
        }

        public void Reset()
        {
            InventoryRepository.dictionary.Clear();
        }

        #endregion
    }
}
