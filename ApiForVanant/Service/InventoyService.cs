using ApiForVanant.Service.Interface;
using System;
using System.Collections.Generic;
using VanantDAL.Repository.Interface;
using VanantModel;

namespace ApiForVanant.Service
{
    /// <summary>
    /// Inventory service:
    /// It adds and takes inventoy items from our repository.
    /// It is needed to inject IInventoryRepository if instantiated. 
    /// If you´d prefer to have it injected by default use ApiInjector
    /// </summary>
    public class InventoyService : IInventoyService
    {
        private IInventoryRepository inventoryRepository;

        public InventoyService(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        #region Public

        /// <summary>
        /// Notifications:
        /// Notifies when InventoryItem has taken out
        /// and
        /// Notifies when InventoryItem has expired
        /// </summary>
        public event NotifyEventHandler Notify;

        /// <summary>
        /// Adds a new inventory item
        /// If repeated does nothing
        /// </summary>
        public void Add(InventoryItem invetoryItem)
        {
            if (invetoryItem == null)
            {
                throw new ArgumentNullException();
            }

            CheckExpiredItems();

            try { 
                this.inventoryRepository.Add(invetoryItem);
            }
            catch (Exception e)
            {
                throw new SystemException(string.Format("An internal error ocurred. Parameters: invetoryItem.Label=\"{0}\", invetoryItem.ExpirationDate=\"{1}\"",
                    invetoryItem.Label, invetoryItem.ExpirationDate), e);
            }
        }
        
        /// <summary>
        /// Takes a inventory item from our repository.
        /// Once taked it is not longer in our repository
        /// </summary>
        /// <param name="label">Identifies the inventoy item</param>
        /// <returns>
        /// Inventory item demande. 
        /// Null if it didn't exist
        /// </returns>
        public InventoryItem Take(string label)
        {
            CheckExpiredItems();

            try
            {
                var inventoryItem = this.inventoryRepository.GetByLabel(label);

                if (inventoryItem != null)
                {
                    this.inventoryRepository.DeleteByLabel(label);

                    if (Notify != null)
                    {
                        Notify(string.Format("Inventoy item with label = \"{0}\" has been taken and removed from our inventory", label));
                    }
                }

                return inventoryItem;
            }
            catch (Exception e)
            {
                throw new SystemException(string.Format("An internal error ocurred. Parameters: label=\"{0}\"", label), e);
            }            
        }

        /// <summary>
        /// Resets the inventoy.
        /// Needed just for testing for now
        /// </summary>
        public void Reset()
        {
            this.inventoryRepository.Reset();
        }

        #endregion

        #region Private
        private void CheckExpiredItems()
        {
            List<InventoryItem> expired = this.inventoryRepository.GetByLessExpiration(DateTime.Now);

            foreach (InventoryItem oneItem in expired)
            {
                if (Notify != null)
                    Notify(string.Format("Inventoy item with label = \"{0}\" has expired", oneItem.Label));

                this.inventoryRepository.DeleteByLabel(oneItem.Label);
            }
        }
        #endregion
    }
}
