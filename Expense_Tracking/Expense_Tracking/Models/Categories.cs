using System;
using System.Collections.Generic;
using System.Text;

namespace Expense_Tracking.Models
{
    public class Categories
    {
        public String CategoryName { get; set; }
        public int CategoryAmount { get; set; }
        public String Image{ get; set; }

        public int Percentage { get; set; }
    }
}
