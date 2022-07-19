using Expense_Tracking.Models;
using Microcharts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;

namespace Expense_Tracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Analysis : ContentPage   {

        public Analysis()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            List<Entry> entries = new List<Entry>();
            string[] color = { "c86d8e", "cb732c", "#4cbb93", "4c8bf5", "deba19", "c71585" };
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
            for (int i = 0; i < categories.Count; i++)
            {
                entries.Add(new Entry(categories[i].CategoryAmount)
                {
                    Label = categories[i].CategoryName,
                    ValueLabel = categories[i].Percentage.ToString() + "%" + "  $" + categories[i].CategoryAmount.ToString(),
                    Color = SkiaSharp.SKColor.Parse(color[i]),
                    ValueLabelColor = SkiaSharp.SKColor.Parse(color[i])
                });
            }
            chartViewPie.Chart = new PieChart { Entries = entries, HoleRadius = 0.3f, LabelTextSize = 40 };
            BudgetInfo.Text = $"Total Budget : {ExpenseManagement.TotalBudget().ToString()}" + "\n"
                + $"Total Expense is: {ExpenseManagement.TotalExpense().ToString()}" + "\n"
                + $"Current Balance is: {ExpenseManagement.CurrentBalance().ToString()}" + "\n";


        }
    }
}