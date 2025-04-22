using System;

namespace LoanManagementSystem.entity
{
    public class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public int CarValue { get; set; }


        public CarLoan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanStatus, string carModel, int carValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanStatus, "CarLoan") // calling base class constructor
        {
            CarModel = carModel;
            CarValue = carValue;
        }
    }

}
