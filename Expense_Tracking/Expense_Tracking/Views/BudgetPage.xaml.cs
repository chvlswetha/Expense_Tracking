using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracking.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Xml.Serialization;
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
            /*BudgetInfo.Text = $"Total Budget : {ExpenseManagement.TotalBudget().ToString()}" + "\n"
                + $"Total Expense is: {ExpenseManagement.TotalExpense().ToString()}" + "\n"
                + $"Current Balance is: {ExpenseManagement.CurrentBalance().ToString()}" + "\n";
            SaveButton.IsVisible = false;
            DeleteButton.IsVisible = false;
            editor.IsVisible = false;*/

        }

        int totalBudget = 0;

        

        protected override void OnAppearing()
        {
            var budgetfile_exs = new Budget();
            budgetfile_exs.FileName = Path.Combine(Environment.GetFolderPath(
                                 Environment.SpecialFolder.LocalApplicationData),
                                                               "budget.txt");

            if (File.Exists(budgetfile_exs.FileName))
            {
                budgetfile_exs.Text = File.ReadAllText(budgetfile_exs.FileName);
                budgetfile_exs.Date = File.GetCreationTime(budgetfile_exs.FileName);
                BudgetInfo.Text = $"Total Budget : {ExpenseManagement.TotalBudget().ToString()}" + "\n"
               + $"Total Expense is: {ExpenseManagement.TotalExpense().ToString()}" + "\n"
                + $"Current Balance is: {ExpenseManagement.CurrentBalance().ToString()}" + "\n";
                SaveButton.IsVisible = false;
                DeleteButton.IsVisible = false;
                editor.IsVisible = false;
                AddMoreBudget.IsVisible = true;
                BudgetCategories.IsVisible = true;
                AddNewExpense.IsVisible = true;
                if (ExpenseManagement.CurrentBalance() < 0)
                    budgetover.IsVisible = true;              
                else
                    budgetover.IsVisible = false;
            }
            else
            {
                SaveButton.IsVisible = true;
                DeleteButton.IsVisible = true;
                editor.IsVisible = true;
                AddMoreBudget.IsVisible = false;
                BudgetCategories.IsVisible = false;
                AddNewExpense.IsVisible = false;
            }
                List<Expense> expenselist = new List<Expense>();
                List<Categories> categories = new List<Categories>();
                Dictionary<string, int> d = new Dictionary<string, int>();
                var path = Environment.GetFolderPath(
                      Environment.SpecialFolder.LocalApplicationData) + "//Expense.xml";
                if (File.Exists(path))
                {
                    Stream Expensefile1 = new FileStream(path, FileMode.Open);
                    XmlSerializer reader = new XmlSerializer(typeof(List<Expense>));
                    expenselist = (List<Expense>)reader.Deserialize(Expensefile1);
                    Expensefile1.Close();
                    foreach (var expense in expenselist)
                    {
                        if (d.ContainsKey(expense.ExpCategory))
                        {
                            d[expense.ExpCategory] += Int32.Parse(expense.ExpAmount);

                        }
                        else
                        {
                            d.Add(expense.ExpCategory, Int32.Parse(expense.ExpAmount));
                        }
                        // totalExpense += Int32.Parse(expense.ExpAmount);
                        //d:key => category value => sum of amounts
                    }
                    foreach (var cate in d)
                    {
                        categories.Add(new Categories
                        {
                            CategoryName = cate.Key,
                            CategoryAmount = cate.Value,
                            Image = cate.Key.ToLower() + ".png",
                            Percentage = (int)Math.Round((double)(100 * cate.Value) / ExpenseManagement.TotalExpense())
                        }); ;
                    }
                }
                 //display aggregated category view
                var allExpenses = ExpenseManagement.GetAllExpense();
                System.Diagnostics.Debug.WriteLine("test log");
                //var categories = ExpenseManagement.CategoryAmount(allExpenses);
                CategoryListView.ItemsSource = categories;
                Analysis AnalysisPage = new Analysis();
                AnalysisPage.BindingContext = categories;
          
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var budget = new Budget();

            if (editor.Text != null)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(editor.Text, "^[0-9]"))
                {
                    await DisplayAlert("Alert", "Enter Valid Budget Amount", "OK");
                    editor.Text = "";
                    return;
                }
                else
                {
                    budget.FileName = Path.Combine(Environment.GetFolderPath(
                                                     Environment.SpecialFolder.LocalApplicationData),
                                                                                   "budget.txt");
                    if (File.Exists(budget.FileName))
                    {
                        budget.Text = File.ReadAllText(budget.FileName);
                        totalBudget += int.Parse(budget.Text);
                    }
                    totalBudget += int.Parse(editor.Text);
                    File.WriteAllText(budget.FileName, totalBudget.ToString());
                    totalBudget = 0;
                    SaveButton.IsVisible = false;
                    DeleteButton.IsVisible = false;
                    AddMoreBudget.IsVisible = true;
                    editor.Text = "";
                    editor.IsVisible = false;
                    BudgetInfo.Text = $"Total Budget : {ExpenseManagement.TotalBudget().ToString()}" + "\n"
                        + $"Total Expense is: {ExpenseManagement.TotalExpense().ToString()}" + "\n"
                         + $"Current Balance is: {ExpenseManagement.CurrentBalance().ToString()}" + "\n";

                    if (ExpenseManagement.CurrentBalance() < 0)
                        budgetover.IsVisible = true;
                    else
                        budgetover.IsVisible = false;
                }
            }
           
       }
        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            editor.Text = "";
            SaveButton.IsVisible = false;
            DeleteButton.IsVisible = false;
            editor.IsVisible = false;
            AddMoreBudget.IsVisible = true;
        }

        private void AddMoreBudget_Clicked(object sender, EventArgs e)
        {
            //var budgetfile_exs = (Budget)BindingContext;
            //budgetfile_exs.FileName = Path.Combine(Environment.GetFolderPath(
            //                     Environment.SpecialFolder.LocalApplicationData),
            //                                                   "budget.txt");
            //budgetfile_exs.Text = File.ReadAllText(budgetfile_exs.FileName);
            //budgetfile_exs.Date = File.GetCreationTime(budgetfile_exs.FileName);
            SaveButton.IsVisible = true;
            DeleteButton.IsVisible = true;
            AddMoreBudget.IsVisible = false;
            editor.IsVisible = true;
            BudgetInfo.Text = "Total Budget : " + $"{ExpenseManagement.TotalBudget().ToString()}" + "\n"
               + $"Total Expense is: {ExpenseManagement.TotalExpense().ToString()}" + "\n"
                + $"Current Balance is: {ExpenseManagement.CurrentBalance().ToString()}" + "\n";
        }

        private async void CategoryListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new CategoryDetail
            {
                BindingContext = (Categories)e.SelectedItem
            });
        }

        private async void  AddNewExpense_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewExpense());
           
        }
    }
}