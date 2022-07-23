# Expense_Tracking
Expense_Tracking_Project
This is Expense Tracking Mobile Application
Technical : Xamarin form - C#

Githib link: https://github.com/chvlswetha/Expense...

 As soon as we launch the application main page opens
1) BudgetPage.Xaml - Where the page list the all the details
For the first time the page asks for the monthly budget to be entered.

i) Save button - Saves the page and budget will be saved in Budget.xml file
ii) Delete - will not save the budget.

Once, saved the page will list, the 
a)TotalBudget
b)TotalExpense and
c)Balance

There is a button to add more budget  
(iv)"Add More Budget" - When clicked we can add amount can be added for budget.

v) Add New Expense - Button when clicked it will open Expense page where the expense can be entered.

2) NewExpense.Xaml -It will have following fields to be entered.
i) Expense Name- we can enter the name of the expense
ii) Expense Date- we can enter the date of the expense
iii) Expense Amount- we can enter the amount of the expense
iv) Expense Category- Need to select in the list of categories - Travel, Food, Education, HomeCare, Miscellaneous etc..
v) Save - will save the expense entered into Expense.xml
vi) Cancel - The expense entered will not be cancelled.

Once Saved, the expense will be saved and returned to mainpage (BudgetPage.Xaml)

3) On BudgetPage.Xaml - The expense saved will return to this page and listed under Budget Categories - All Expenses of particular category amounts are summed up and displayed as Expense.

When Clicked on each category , 

4) CategoryDetails.Xaml page opens and list all expenses under category
With 2 new buttons EDIT and Delete

i) Edit button when clicked opens NewExpense.Xaml page with Values populated. We can go and edit whatever value we want and SAVE. The new edited values will be saved and the expsne amount changes if any will be recalculated.

ii) Delete button when clicked will delete the particular expense and amount will be recalculated

5)Analysis.Xml - Shows the Pictorial representation of the expense in percentages through Pie Chart.
