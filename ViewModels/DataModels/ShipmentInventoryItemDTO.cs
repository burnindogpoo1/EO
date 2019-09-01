using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class ShipmentInventoryItemDTO
    {
        public ShipmentInventoryItemDTO()
        {

        }

        public ShipmentInventoryItemDTO(long shipmentId, long inventoryId, string inventoryName, long imageId, int quantity = 1)
        {
            ShipmentId = shipmentId;
            InventoryId = inventoryId;
            InventoryName = inventoryName;
            ImageId = imageId;
            Quantity = quantity;
        }
        public long ShipmentId { get; set; }

        public long InventoryId { get; set; }

        public string InventoryName { get; set; }

        public int Quantity { get; set; }

        public long ImageId { get; set; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
