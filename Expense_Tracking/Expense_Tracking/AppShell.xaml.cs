using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Expense_Tracking.Models;

namespace Expense_Tracking
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Aboutpage.BindingContext = new Budget();
        }

    }
}
