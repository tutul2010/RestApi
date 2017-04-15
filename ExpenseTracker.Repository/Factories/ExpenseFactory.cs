using ExpenseTracker.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ExpenseTracker.Repository.Factories
{
    //Mapping cls btwn  DTO and entity
    public class ExpenseFactory
    {

        public ExpenseFactory()
        {

        }

        public DTO.Expense CreateExpense(Expense expense)
        {
            return new DTO.Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }



        public Expense CreateExpense(DTO.Expense expense)
        {
            return new Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }
        //for fields level data shaping 
        public object CreateDataShapedObject(Expense expense, List<string> lstOfFields)
        {
            //pass through from entity to DTO
            return CreateDataShapedObject(CreateExpense(expense), lstOfFields);
        }

        private object CreateDataShapedObject(DTO.Expense expense, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return expense;
            }
            else
            {

                // create a new ExpandoObject & dynamically create the properties for this object

                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    // need to include public and instance, b/c specifying a binding flag overwrites the
                    // already-existing binding flags.

                    var fieldValue = expense.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(expense, null);

                    // add the field to the ExpandoObject
                    ((IDictionary<String, Object>)objectToReturn).Add(field, fieldValue);
                }

                return objectToReturn;
            }
        }
    }
}
