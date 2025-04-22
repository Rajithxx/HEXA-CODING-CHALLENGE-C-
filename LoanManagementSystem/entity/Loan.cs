using System;

namespace LoanManagementSystem.entity
{
    public abstract class Loan
    {
        public int LoanId { get; set; }
        public Customer Customer { get; set; }
        public double PrincipalAmount { get; set; }
        public double InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanStatus { get; set; }
        public string LoanType { get; set; }

        
        protected Loan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanStatus, string loanType)
        {
            LoanId = loanId;
            Customer = customer;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanStatus = loanStatus;
            LoanType = loanType;
        }

        
        public void DisplayLoanInfo()
        {
            Console.WriteLine($"Loan ID: {LoanId}, Customer: {Customer.Name}, Principal Amount: {PrincipalAmount}, Interest Rate: {InterestRate}, Loan Term: {LoanTerm}, Loan Status: {LoanStatus}");
        }
    }
}
