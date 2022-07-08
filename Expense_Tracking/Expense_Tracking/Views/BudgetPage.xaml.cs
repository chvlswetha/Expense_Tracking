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

        int totalBudget = 0;
        int totalExpense = 0;
        

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
                BudgetInfo.Text = "Your Budget set for this month is:" + budgetfile_exs.Text + "\n"
                    + $"Total Expense : {ExpenseManagement.TotalExpense().ToString()}" +"\n"
                    + $"Current Balance : {ExpenseManagement.CurrentBalance().ToString()}" + "\n";

                SaveButton.IsVisible = false;
                DeleteButton.IsVisible = false;
                editor.IsVisible = false;
            }

            //display aggregated category view
            var allExpenses = ExpenseManagement.GetAllExpense();
            System.Diagnostics.Debug.WriteLine("test log");
            var categories = ExpenseManagement.CategoryAmount(allExpenses);
            CategoryListView.ItemsSource = categories;
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            int totalExpense = ExpenseManagement.TotalExpense();
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
                    totalBudget += int.Parse(budget.Text);
                }
                totalBudget += int.Parse(editor.Text);
                File.WriteAllText(budget.FileName, totalBudget.ToString());

            }
            string newBudget = File.ReadAllText(budget.FileName);
            
            
            SaveButton.IsVisible = false;
            DeleteButton.IsVisible = false;
            editor.IsVisible = false;
            BudgetInfo.Text = "Total Budget : " + newBudget + "\n"
                + $"Total Expense is: {ExpenseManagement.TotalExpense().ToString()}" + "\n"
                 + $"Current Balance is: {ExpenseManagement.CurrentBalance().ToString()}" + "\n";
            await Task.Delay(5000);
            //await Navigation.PopModalAsync();
        }
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            editor.Text = "";
            await Task.Delay(5000);
            
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