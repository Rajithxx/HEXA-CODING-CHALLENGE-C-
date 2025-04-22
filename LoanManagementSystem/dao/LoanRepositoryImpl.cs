using LoanManagementSystem.entity;
using LoanManagementSystem.util;
using LoanManagementSystem.exception;
using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace LoanManagementSystem.dao
{
    public class LoanRepositoryImpl : ILoanRepository
    {
        public void ApplyLoan(Loan loan)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("INSERT INTO LoanSystem.Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) VALUES (@CustomerId, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus)", connection);
                command.Parameters.AddWithValue("@CustomerId", loan.Customer.CustomerId);
                command.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                command.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                command.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                command.Parameters.AddWithValue("@LoanType", loan.LoanType);
                command.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Loan applied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while applying loan: {ex.Message}");
                }
            }
        }

        public double CalculateInterest(int loanId)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT PrincipalAmount, InterestRate, LoanTerm FROM LoanSystem.Loan WHERE LoanId = @LoanId", connection);
                command.Parameters.AddWithValue("@LoanId", loanId);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double principal = reader.GetDouble(0);
                            double rate = reader.GetDouble(1)/100;
                            int term = reader.GetInt32(2);

                            return (principal * rate * term) / 12;
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found!");
                        }
                    }
                }
                catch (InvalidLoanException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public double CalculateInterest(double principal, double rate, int term)
        {
            return (principal * rate * term) / 12;
        }

        public void LoanStatus(int loanId)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT CustomerId FROM LoanSystem.Loan WHERE LoanId = @LoanId", connection);
                command.Parameters.AddWithValue("@LoanId", loanId);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int customerId = reader.GetInt32(0);
                            var creditScore = GetCustomerCreditScore(customerId);
                            string loanStatus = creditScore >= 650 ? "Approved" : "Rejected";
                            UpdateLoanStatus(loanId, loanStatus);
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found!");
                        }
                    }
                }
                catch (InvalidLoanException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public int GetCustomerCreditScore(int customerId)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT CreditScore FROM LoanSystem.Customer WHERE CustomerId = @CustomerId", connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        throw new InvalidLoanException("Customer not found!");
                    }
                }
            }
        }

        private void UpdateLoanStatus(int loanId, string status)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("UPDATE LoanSystem.Loan SET LoanStatus = @LoanStatus WHERE LoanId = @LoanId", connection);
                command.Parameters.AddWithValue("@LoanStatus", status);
                command.Parameters.AddWithValue("@LoanId", loanId);

                command.ExecuteNonQuery();
                Console.WriteLine($"Loan Status Updated: {status}");
            }
        }

        public double CalculateEMI(int loanId)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT PrincipalAmount, InterestRate, LoanTerm FROM LoanSystem.Loan WHERE LoanId = @LoanId", connection);
                command.Parameters.AddWithValue("@LoanId", loanId);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double principal = reader.GetDouble(0);
                            double rate = reader.GetDouble(1) / 12 / 100;
                            int term = reader.GetInt32(2);

                            return (principal * rate * Math.Pow(1 + rate, term)) / (Math.Pow(1 + rate, term) - 1);
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found!");
                        }
                    }
                }
                catch (InvalidLoanException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public double CalculateEMI(double principal, double rate, int term)
        {
            rate = rate / 12 / 100;
            return (principal * rate * Math.Pow(1 + rate, term)) / (Math.Pow(1 + rate, term) - 1);
        }

        public void LoanRepayment(int loanId, double amount)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT PrincipalAmount, LoanTerm FROM LoanSystem.Loan WHERE LoanId = @LoanId", connection);
                command.Parameters.AddWithValue("@LoanId", loanId);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double principal = reader.GetDouble(0);
                            int term = reader.GetInt32(1);

                            double emiAmount = CalculateEMI(principal, 6.5, term);
                            if (amount < emiAmount)
                            {
                                Console.WriteLine("Amount is less than a single EMI, repayment rejected.");
                            }
                            else
                            {
                                double remainingBalance = principal - amount;

                                reader.Close();

                                var updateCommand = new SqlCommand("UPDATE LoanSystem.Loan SET PrincipalAmount = @RemainingBalance WHERE LoanId = @LoanId", connection);
                                updateCommand.Parameters.AddWithValue("@RemainingBalance", remainingBalance);
                                updateCommand.Parameters.AddWithValue("@LoanId", loanId);

                                updateCommand.ExecuteNonQuery();
                                Console.WriteLine($"Repayment successful. Remaining Balance: {remainingBalance}");
                            }
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found!");
                        }
                    }
                }
                catch (InvalidLoanException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT LoanId, CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanStatus, LoanType, PropertyAddress, PropertyValue, CarModel, CarValue FROM LoanSystem.Loan", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int loanId = reader.GetInt32(0);
                        int customerId = reader.GetInt32(1);
                        double principalAmount = reader.GetDouble(2);
                        double interestRate = reader.GetDouble(3);
                        int loanTerm = reader.GetInt32(4);
                        string loanStatus = reader.GetString(5);
                        string loanType = reader.GetString(6);
                        string propertyAddress = reader.IsDBNull(7) ? null : reader.GetString(7);
                        double? propertyValue = reader.IsDBNull(8) ? (double?)null : reader.GetDouble(8);
                        string carModel = reader.IsDBNull(9) ? null : reader.GetString(9);
                        double? carValue = reader.IsDBNull(10) ? (double?)null : reader.GetDouble(10);

                        Loan loan = null;
                        if (loanType == "HomeLoan")
                        {
                            loan = new HomeLoan(loanId, GetCustomerById(customerId), principalAmount, interestRate, loanTerm, loanStatus, propertyAddress, (int)(propertyValue ?? 0));
                        }
                        else if (loanType == "CarLoan")
                        {
                            loan = new CarLoan(loanId, GetCustomerById(customerId), principalAmount, interestRate, loanTerm, loanStatus, carModel, (int)(carValue ?? 0));
                        }
                        loans.Add(loan);
                    }
                }
            }
            return loans;
        }

        public Loan GetLoanById(int loanId)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT LoanId, CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanStatus, LoanType, PropertyAddress, PropertyValue, CarModel, CarValue FROM LoanSystem.Loan WHERE LoanId = @LoanId", connection);
                command.Parameters.AddWithValue("@LoanId", loanId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int customerId = reader.GetInt32(1);
                        double principalAmount = reader.GetDouble(2);
                        double interestRate = reader.GetDouble(3);
                        int loanTerm = reader.GetInt32(4);
                        string loanStatus = reader.GetString(5);
                        string loanType = reader.GetString(6);
                        string propertyAddress = reader.IsDBNull(7) ? null : reader.GetString(7);
                        double? propertyValue = reader.IsDBNull(8) ? (double?)null : reader.GetDouble(8);
                        string carModel = reader.IsDBNull(9) ? null : reader.GetString(9);
                        double? carValue = reader.IsDBNull(10) ? (double?)null : reader.GetDouble(10);

                        if (loanType == "HomeLoan")
                        {
                            return new HomeLoan(loanId, GetCustomerById(customerId), principalAmount, interestRate, loanTerm, loanStatus, propertyAddress, (int)(propertyValue ?? 0));
                        }
                        else if (loanType == "CarLoan")
                        {
                            return new CarLoan(loanId, GetCustomerById(customerId), principalAmount, interestRate, loanTerm, loanStatus, carModel, (int)(carValue ?? 0));
                        }
                    }
                }
            }

            throw new InvalidLoanException("Loan not found!");
        }

        private Customer GetCustomerById(int customerId)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT * FROM LoanSystem.Customer WHERE CustomerId = @CustomerId", connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(1);
                        string address = reader.GetString(2);
                        string email = reader.GetString(3);
                        string phone = reader.GetString(4);
                        int creditScore = reader.GetInt32(5);

                        return new Customer(customerId, name, address, email, phone, creditScore);
                    }
                }
            }

            throw new InvalidLoanException("Customer not found!");
        }
    }
}
