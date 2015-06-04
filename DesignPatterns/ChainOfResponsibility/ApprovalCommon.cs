using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.ChainOfResponsibility
{
    public interface IExpenseReport
    {
        Decimal Total { get; }
    }

    public interface IExpenseApprover //employee who can approve
    {
        ApprovalResponse ApproveExpense(IExpenseReport expenseReport);
    }

    public enum ApprovalResponse
    {
        Denied,
        Approved,
        BeyondApprovalLimit
    }

    public class ExpenseReport
    {
        public Decimal Total { get; set; }
    }


    public class Employee : IExpenseApprover
    {
        private readonly decimal _approvalLimit;

        public Employee(string name,Decimal approvalLimit)
        {
            Name = name;
            _approvalLimit = approvalLimit;
        }

        public string Name { get; private set; }

        public ApprovalResponse ApproveExpense(IExpenseReport expenseReport)
        {
            return expenseReport.Total > _approvalLimit
                ? ApprovalResponse.BeyondApprovalLimit
                : ApprovalResponse.Approved;
        }
    }

    public class Approval
    {
        static void NonChainOfREsponsibility()
        {
            List<Employee> managers=new List<Employee>
            {
                new Employee("William Worker",0),
                new Employee("Mary Manager",new Decimal(1000)),
                new Employee("Victor VicePres",new Decimal(5000)),
                new Employee("Paula Pres",new Decimal(20000))
            };

            Decimal expenseReportAmount;
            while (ConsoleInput.TryReadDecimal("Expense Report amount:",out expenseReportAmount))
            {
                IExpenseReport expense = new ExpenseReport(expenseReportAmount);
                bool expenseProcessed = false;

                foreach (var approver in managers)
                {
                    ApprovalResponse response = approver.ApproveExpense(expense);
                    if (response != ApprovalResponse.BeyondApprovalLimit)
                    {
                        Console.WriteLine("The request was {0}",response);
                        expenseProcessed = true;
                        break;
                    }
                }
                if (!expenseProcessed)
                {
                    Console.WriteLine("No one was able to approve expense");
                }
            }
        }
    }
}
