using System;

namespace LoanManagementSystem.entity
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }


        public HomeLoan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanStatus, "HomeLoan") // calling base class constructor
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }
    }
}
