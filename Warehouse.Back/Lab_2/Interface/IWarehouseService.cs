using Warehouse.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Back.Interface
{
    public interface IWarehouseService
    {
        void ReceiveGoods(string sku, string name, int quantity, decimal pricePerUnit, string supplier, DateTime deliveryDate);
        void WriteOffGoods(string sku, int quantity, string reason, DateTime writeOffDate);
        void InventoryAdjustment(string sku, int actualQuantity, string reason, DateTime adjustmentDate);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<OperationRecord> GetAllOperations();
        decimal GetTotalInventoryValue();
    }
}
