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
using System.Collections.ObjectModel;

namespace Expense_Tracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExpense : ContentPage
    {
        
        List<Expense> expenselist = new List<Expense>();
        Expense editExpToBeDelete;
        public NewExpense()
        {
            InitializeComponent();
  
        }    

        protected override void OnAppearing()
        {            

            var editExp = (Expense)BindingContext;
            editExpToBeDelete = new Expense{ ExpName = editExp.ExpName, ExpAmount = editExp.ExpAmount, 
                ExpCategory = editExp.ExpCategory, ExpDate = editExp.ExpDate};

            if (editExp.ExpName != null)
            {
       
                ExpName.Text = editExp.ExpName;
                ExpDate.Date = DateTime.Parse(editExp.ExpDate);
                ExpAmount.Text = editExp.ExpAmount;
                ExpCategory.SelectedItem = editExp.ExpCategory;

                
            }
        }

        private async void SaveExpense_Clicked(object sender, EventArgs e)
        {
            var expense = (Expense)BindingContext;
            List<Expense> expenselist1 = new List<Expense>();

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
                expenselist1 = (List<Expense>)reader.Deserialize(Expensefile1);
                Expensefile1.Close();
            }
            Stream Expensefile = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            XmlSerializer writer = new XmlSerializer(typeof(List<Expense>));            
            
            expenselist1.Add(expense);
            if (editExpToBeDelete.ExpName != null)
            {
                var exs = expenselist1.Find(exp =>
            (exp.ExpName == editExpToBeDelete.ExpName && exp.ExpAmount == editExpToBeDelete.ExpAmount
            && exp.ExpDate == editExpToBeDelete.ExpDate && exp.ExpCategory == editExpToBeDelete.ExpCategory));

                expenselist1.Remove(exs);
            }
            writer.Serialize(Expensefile, expenselist1);
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