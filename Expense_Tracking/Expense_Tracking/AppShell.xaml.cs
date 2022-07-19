using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Expense_Tracking.Models;
using Expense_Tracking.Views;

namespace Expense_Tracking
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Home.BindingContext = new Budget();
            Analysis.BindingContext = new Analysis();
         }

    }
}
