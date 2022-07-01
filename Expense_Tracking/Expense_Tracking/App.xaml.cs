using System;
using Expense_Tracking.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Expense_Tracking
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
