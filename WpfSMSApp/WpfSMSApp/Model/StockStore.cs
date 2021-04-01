using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSMSApp.Model
{
    public class StockStore
    {
        public int StockQuantity { get; set; }
        public int StoreID { get; internal set; }
        public string StoreName { get; internal set; }
        public string StoreLocation { get; internal set; }
        public bool ItemStatus { get; internal set; }
        public int TagID { get; internal set; }
        public int BarcodeID { get; internal set; }
    }
}
