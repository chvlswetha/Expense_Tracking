using Expense_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Expense_Tracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryDetail : ContentPage
    {
        ObservableCollection<Expense> expenselist = new ObservableCollection<Expense>();
        
        public CategoryDetail()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            var cate = (Categories)BindingContext;
            //expensetype.Text = cate.CategoryName + " Expense";
            string path = Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData) + "//Expense.xml";
            if (File.Exists(path))
            {
                Stream Expensefile1 = new FileStream(path, FileMode.Open);
                XmlSerializer reader = new XmlSerializer(typeof(ObservableCollection<Expense>));
                expenselist = (ObservableCollection<Expense>)reader.Deserialize(Expensefile1);
                Expensefile1.Close();
            }
            var filteredExpanse = expenselist.Where(x => x.ExpCategory == cate.CategoryName);    

            foreach (Expense expense in filteredExpanse)
            {
                //string[] splitText = expense.ExpDate.Split(new char[] { ' ' });
                //expense.ExpDate = splitText[0];
            }
            ExpenseDetail.Text = $"{cate.CategoryName} Expense Detail";
            CategoryDetailListView.ItemsSource = filteredExpanse;
         
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Expense editExp = b.BindingContext as Expense;
            await Navigation.PushAsync(new NewExpense
            {
                BindingContext = editExp,
            });
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Expense deleExp = b.BindingContext as Expense;

            var path = Environment.GetFolderPath(
                  Environment.SpecialFolder.LocalApplicationData) + "//Expense.xml";
            if (File.Exists(path))
            {
                Stream Expensefile1 = new FileStream(path, FileMode.Open);
                XmlSerializer reader = new XmlSerializer(typeof(List<Expense>));
                List<Expense> expenselist = (List<Expense>)reader.Deserialize(Expensefile1);
                Expensefile1.Close();

                var exs = expenselist.Find(exp =>
                (exp.ExpName == deleExp.ExpName && exp.ExpAmount == deleExp.ExpAmount
                && exp.ExpDate == deleExp.ExpDate && exp.ExpCategory == deleExp.ExpCategory));

                expenselist.Remove(exs);

                Stream Expensefile = new FileStream(path, FileMode.Create, FileAccess.Write);
                XmlSerializer writer = new XmlSerializer(typeof(List<Expense>));
                writer.Serialize(Expensefile, expenselist);
                Expensefile.Close();

                var filteredExpanse = expenselist.Where(x => x.ExpCategory == deleExp.ExpCategory);

                CategoryDetailListView.ItemsSource = null;
                CategoryDetailListView.ItemsSource = filteredExpanse;
            }
        }
    }
}