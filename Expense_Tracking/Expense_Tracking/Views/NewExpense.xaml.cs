using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracking.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Expense_Tracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExpense : ContentPage
    {
        public NewExpense()
        {
            InitializeComponent();
        }

        private async void SaveExpense_Clicked(object sender, EventArgs e)
        {
            var expense = (Expense)BindingContext;
            List<Expense> expenselist = new List<Expense>();

            var path = Environment.GetFolderPath(
                  Environment.SpecialFolder.LocalApplicationData) + "//Expense.xml";


            expense = new Expense();
            expense.ExpName = ExpName.Text;
            expense.ExpDate =   ExpDate.Date.ToString();
            expense.ExpAmount = ExpAmount.Text;
            expense.ExpCategory = ExpCategory.SelectedItem.ToString();

            if (File.Exists(path))
            {
                Stream Expensefile1 = new FileStream(path, FileMode.Open);
                XmlSerializer reader = new XmlSerializer(typeof(List<Expense>));
                expenselist = (List<Expense>)reader.Deserialize(Expensefile1);
                Expensefile1.Close();
            }
            Stream Expensefile = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            XmlSerializer writer = new XmlSerializer(typeof(List<Expense>));
            expenselist.Add(expense);
            writer.Serialize(Expensefile, expenselist);
            Expensefile.Close();

            SaveExpense.IsEnabled = false;
            await Task.Delay(5000);
            await Navigation.PopModalAsync();
        }

        private void CancelExpense_Clicked(object sender, EventArgs e)
        {

        }
    }
}