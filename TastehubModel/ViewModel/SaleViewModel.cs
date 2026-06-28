using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoModel.ViewModel
{
  public class SaleViewModel
    {
        public int Id { get; set; }
        public System.DateTime DateSold { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }
}
