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
using System.Collections.ObjectModel;

namespace Expense_Tracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetPage : ContentPage
    {
        public BudgetPage()
        {
            InitializeComponent();            

        }

        int totalbudget = 0;

        protected override void OnAppearing()
        {
            var budgetfile_exs = (Budget)BindingContext;
            budgetfile_exs.FileName = Path.Combine(Environment.GetFolderPath(
                                 Environment.SpecialFolder.LocalApplicationData),
                                                               "budget.txt");

            if (File.Exists(budgetfile_exs.FileName))
            {
                budgetfile_exs.Text = File.ReadAllText(budgetfile_exs.FileName);
                budgetfile_exs.Date = File.GetCreationTime(budgetfile_exs.FileName);
                BudgetInfo.Text = "Your Budget set for this month is:" + budgetfile_exs.Text + "\n";
                SaveButton.IsVisible = false;
                DeleteButton.IsVisible = false;
                editor.IsVisible = false;
                BudgetInfo.Text = BudgetInfo.Text + "\n";
                BudgetInfo.Text = BudgetInfo.Text + "Total Expense is: 0";                
            }

            //display aggregated category view
            var allExpenses = ExpenseManagement.GetAllExpense();
            System.Diagnostics.Debug.WriteLine("test log");
            var categories = ExpenseManagement.CategoryAmount(allExpenses);

            //List<Expense> expenselist = new List<Expense>();
            //List<Categories> categories = new List<Categories>();
            //Dictionary<string, int> d = new Dictionary<string, int>();
            //var path = Environment.GetFolderPath(
            //      Environment.SpecialFolder.LocalApplicationData) + "//Expense.xml";
            //if (File.Exists(path))
            //{
            //    Stream Expensefile1 = new FileStream(path, FileMode.Open);
            //    XmlSerializer reader = new XmlSerializer(typeof(List<Expense>));
            //    expenselist = (List<Expense>)reader.Deserialize(Expensefile1);
            //    Expensefile1.Close();
            //    foreach (var expense in expenselist)
            //    {
            //        if (d.ContainsKey(expense.ExpCategory))
            //        {
            //            d[expense.ExpCategory] += Int32.Parse(expense.ExpAmount);
            //        }
            //        else
            //        {
            //            d.Add(expense.ExpCategory, Int32.Parse(expense.ExpAmount));
            //        }

            //        //d:key => category value => sum of amounts
            //    }
            //    foreach (var cate in d)
            //    {
            //        categories.Add(new Categories
            //        {
            //            CategoryName = cate.Key,
            //            CategoryAmount = cate.Value,
            //        });
            //    }
            //}
            CategoryListView.ItemsSource = categories;
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var budget = (Budget)BindingContext;
            if (!System.Text.RegularExpressions.Regex.IsMatch(editor.Text, "^[0-9]"))
            {
                await DisplayAlert("Alert", "Enter Valid Budget Amount", "OK");
                editor.Text = "";
            }
            else
            {
                budget.FileName = Path.Combine(Environment.GetFolderPath(
                                                 Environment.SpecialFolder.LocalApplicationData),
                                                                               "budget.txt");
                if (!string.IsNullOrEmpty(budget.Text))
                {
                    budget.Text = File.ReadAllText(budget.FileName);
                    totalbudget += int.Parse(budget.Text);
                }
                totalbudget += int.Parse(editor.Text);
                File.WriteAllText(budget.FileName, totalbudget.ToString());

            }
            await Task.Delay(5000);
            //await Navigation.PopModalAsync();
        }
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            await Task.Delay(5000);
            await Navigation.PopModalAsync();
        }

        private void AddMoreBudget_Clicked(object sender, EventArgs e)
        {

            var budgetfile_exs = new Budget();
            budgetfile_exs.FileName = Path.Combine(Environment.GetFolderPath(
           Environment.SpecialFolder.LocalApplicationData),
                     "budget.txt");
            budgetfile_exs.Text = File.ReadAllText(budgetfile_exs.FileName);
            budgetfile_exs.Date = File.GetCreationTime(budgetfile_exs.FileName);

            BudgetInfo.Text = "Current Budget : " + budgetfile_exs.Text + "\n";
            SaveButton.IsVisible = true;
            DeleteButton.IsVisible = true;
            editor.IsVisible = true;
            BudgetInfo.Text = BudgetInfo.Text + "\n";
            BudgetInfo.Text = BudgetInfo.Text + "Total Expense is: 0" + "\n";
            BudgetInfo.Text = BudgetInfo.Text + "Add More:";
      }

        private async void CategoryListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new CategoryDetail
            {
                BindingContext = (Categories)e.SelectedItem
            });
        }


    }
}