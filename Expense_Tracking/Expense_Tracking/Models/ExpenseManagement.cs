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
                Stream Expensefile1 = new FileStream(path, FileMode.Open);
                XmlSerializer reader = new XmlSerializer(typeof(ObservableCollection<Expense>));
                try
                {
                    allExpenses = (ObservableCollection<Expense>)reader.Deserialize(Expensefile1);
                    throw new Exception("test ex");
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[LOG]Get all expenses failed, ex:{ex.ToString()}");
                    System.Diagnostics.Debug.WriteLine($"[LOG]Stack trace, ex:{ex.StackTrace}");
                    System.Diagnostics.Debug.WriteLine($"[LOG]iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii*");
                    throw ex;
                }
                finally
                {
                    Expensefile1.Close();
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
    }
}
