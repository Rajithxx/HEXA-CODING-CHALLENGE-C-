using LoanManagementSystem.entity;
using System;
using System.Collections.Generic;

namespace LoanManagementSystem.dao
{
    public interface ILoanRepository
    {
        void ApplyLoan(Loan loan);
        List<Loan> GetAllLoans();
        Loan GetLoanById(int loanId);
        void LoanRepayment(int loanId, double amount);
        double CalculateInterest(int loanId);
        void LoanStatus(int loanId);
        double CalculateEMI(int loanId);
        int GetCustomerCreditScore(int customerId); // Ensure this method is declared in the interface
    }
}
