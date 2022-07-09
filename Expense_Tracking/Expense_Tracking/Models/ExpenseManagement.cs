using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Expense_Tracking.Models
{
    public static class ExpenseManagement
    {
        public static ObservableCollection<Expense> GetAllExpense()
        {
            ObservableCollection<Expense> allExpenses = new ObservableCollection<Expense>();
            var path = Environment.GetFolderPath(
                  Environment.SpecialFolder.LocalApplicationData) + "//Expense.xml";
            if (File.Exists(path))
            {
                Stream expensefile1 = new FileStream(path, FileMode.Open);
                XmlSerializer reader = new XmlSerializer(typeof(ObservableCollection<Expense>));
                try
                {
                    allExpenses = (ObservableCollection<Expense>)reader.Deserialize(expensefile1);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[LOG]Get all expenses failed, ex:{ex}");
                    throw ex;
                }
                finally
                {
                    expensefile1.Close();
                }
                
            }
            return allExpenses;
        }
        public static ObservableCollection<Categories> CategoryAmount(ObservableCollection<Expense> allExpenses)
        {
            ObservableCollection<Categories> categories = new ObservableCollection<Categories>();
            Dictionary<string, int> d = new Dictionary<string, int>();
            foreach (var expense in allExpenses)
            {
                if (d.ContainsKey(expense.ExpCategory))
                {
                    d[expense.ExpCategory] += Int32.Parse(expense.ExpAmount);
                }
                else
                {
                    d.Add(expense.ExpCategory, Int32.Parse(expense.ExpAmount));
                }

                //d:key => category value => sum of amounts
            }
            foreach (var category in d)
            {
                categories.Add(new Categories
                {
                    CategoryName = category.Key,
                    CategoryAmount = category.Value,
                });
            }
            return categories;
        }

        public static int TotalExpense()
        {
            var allExpenses = GetAllExpense();
            int totalExpense = 0;
            for(int i = 0; i < allExpenses.Count; i++)
            {
                totalExpense += Int32.Parse(allExpenses[i].ExpAmount);
            }
            return totalExpense;
        }
        public static int TotalBudget()
        {
            var path = Path.Combine(Environment.GetFolderPath(
                                                 Environment.SpecialFolder.LocalApplicationData),
                                                                               "budget.txt");
            int totalBudget = 0;
            if (File.Exists(path))
            {
                totalBudget = Int32.Parse(File.ReadAllText(path));
            }
            return totalBudget;
        }
        public static int CurrentBalance()
        {
            int currentBalance = 0;
            currentBalance = TotalBudget() - TotalExpense();
            return currentBalance;
        }
       
    }
}
