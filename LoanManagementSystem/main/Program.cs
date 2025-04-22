using LoanManagementSystem.entity;
using LoanManagementSystem.dao;
using LoanManagementSystem.exception;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using LoanManagementSystem.util;

namespace LoanManagementSystem.main
{
    public class LoanManagementMain
    {
        static ILoanRepository loanRepository = new LoanRepositoryImpl();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Loan Management System");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Calculate Loan Interest");
                Console.WriteLine("6. Check Loan Status");
                Console.WriteLine("7. Calculate EMI");
                Console.WriteLine("8. Get Customer Credit Score");
                Console.WriteLine("9. Exit");
                Console.Write("Please select an option (1-9): ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ApplyLoan();
                            break;
                        case 2:
                            GetAllLoans();
                            break;
                        case 3:
                            GetLoanById();
                            break;
                        case 4:
                            LoanRepayment();
                            break;
                        case 5:
                            CalculateInterest();
                            break;
                        case 6:
                            CheckLoanStatus();
                            break;
                        case 7:
                            CalculateEMI();
                            break;
                        case 8:
                            GetCustomerCreditScore();
                            break;
                        case 9:
                            Console.WriteLine("Exiting Loan Management System. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 9.");
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        public static void ApplyLoan()
        {
            try
            {
                Console.WriteLine("\nApply for Loan");

                Console.Write("Enter Customer ID: ");
                int customerId = int.Parse(Console.ReadLine());

                Customer customer = GetCustomerById(customerId);

                Console.Write("Enter Principal Amount: ");
                double principalAmount = double.Parse(Console.ReadLine());

                Console.Write("Enter Interest Rate (e.g., 6.5): ");
                double interestRate = double.Parse(Console.ReadLine());

                Console.Write("Enter Loan Term (in months): ");
                int loanTerm = int.Parse(Console.ReadLine());

                Console.Write("Enter Loan Type (HomeLoan / CarLoan): ");
                string loanType = Console.ReadLine();

                Loan loan;

                if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Property Address: ");
                    string propertyAddress = Console.ReadLine();

                    Console.Write("Enter Property Value: ");
                    int propertyValue = int.Parse(Console.ReadLine());

                    loan = new HomeLoan(0, customer, principalAmount, interestRate, loanTerm, "Pending", propertyAddress, propertyValue);
                }
                else if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Car Model: ");
                    string carModel = Console.ReadLine();

                    Console.Write("Enter Car Value: ");
                    int carValue = int.Parse(Console.ReadLine());

                    loan = new CarLoan(0, customer, principalAmount, interestRate, loanTerm, "Pending", carModel, carValue);
                }
                else
                {
                    Console.WriteLine("Invalid loan type.");
                    return;
                }

                loanRepository.ApplyLoan(loan);
                Console.WriteLine("Loan application submitted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error applying loan: " + ex.Message);
            }
        }

        public static Customer GetCustomerById(int customerId)
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("SELECT CustomerId, Name, Email, PhoneNumber, Address, CreditScore FROM LoanSystem.Customer WHERE CustomerId = @CustomerId", connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(1);
                        string email = reader.GetString(2);
                        string phoneNumber = reader.GetString(3);
                        string address = reader.GetString(4);
                        int creditScore = reader.GetInt32(5);

                        return new Customer(customerId, name, email, phoneNumber, address, creditScore);
                    }
                    else
                    {
                        throw new InvalidLoanException("Customer not found!");
                    }
                }
            }
        }

        public static void GetAllLoans()
        {
            try
            {
                Console.WriteLine("\nFetching All Loans...");
                List<Loan> loans = loanRepository.GetAllLoans();

                if (loans.Count == 0)
                {
                    Console.WriteLine("No loans found.");
                }
                else
                {
                    foreach (var loan in loans)
                    {
                        Console.WriteLine($"Loan ID: {loan.LoanId}, Customer ID: {loan.Customer.CustomerId}, Principal: {loan.PrincipalAmount}, Status: {loan.LoanStatus}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching loans: " + ex.Message);
            }
        }

        public static void GetLoanById()
        {
            try
            {
                Console.WriteLine("\nEnter Loan ID to Fetch:");
                int loanId = int.Parse(Console.ReadLine());

                Loan loan = loanRepository.GetLoanById(loanId);
                if (loan != null)
                {
                    Console.WriteLine($"Loan ID: {loan.LoanId}");
                    Console.WriteLine($"Customer: {loan.Customer.Name}, Address: {loan.Customer.Address}");
                    Console.WriteLine($"Principal: {loan.PrincipalAmount}, Interest Rate: {loan.InterestRate}, Term: {loan.LoanTerm} months");
                    Console.WriteLine($"Loan Status: {loan.LoanStatus}");
                }
                else
                {
                    Console.WriteLine("Loan not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching loan: " + ex.Message);
            }
        }

        public static void LoanRepayment()
        {
            try
            {
                Console.WriteLine("\nLoan Repayment");

                Console.Write("Enter Loan ID: ");
                int loanId = int.Parse(Console.ReadLine());

                Console.Write("Enter Repayment Amount: ");
                double amount = double.Parse(Console.ReadLine());

                loanRepository.LoanRepayment(loanId, amount);
                Console.WriteLine("Repayment successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error making repayment: " + ex.Message);
            }
        }

        public static void CalculateInterest()
        {
            try
            {
                Console.WriteLine("\nEnter Loan ID to Calculate Interest:");
                int loanId = int.Parse(Console.ReadLine());

                double interest = loanRepository.CalculateInterest(loanId);
                Console.WriteLine($"Interest for Loan ID {loanId}: {interest}");
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void CheckLoanStatus()
        {
            try
            {
                Console.WriteLine("\nEnter Loan ID to Check Status:");
                int loanId = int.Parse(Console.ReadLine());

                loanRepository.LoanStatus(loanId);
                Console.WriteLine("Loan status updated successfully.");
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void CalculateEMI()
        {
            try
            {
                Console.WriteLine("\nEnter Loan ID to Calculate EMI:");
                int loanId = int.Parse(Console.ReadLine());

                double emi = loanRepository.CalculateEMI(loanId);
                Console.WriteLine($"EMI for Loan ID {loanId}: {emi}");
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void GetCustomerCreditScore()
        {
            try
            {
                Console.Write("Enter Customer ID: ");
                int customerId = int.Parse(Console.ReadLine());

                int score = loanRepository.GetCustomerCreditScore(customerId);
                Console.WriteLine($"Customer's Credit Score: {score}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching credit score: " + ex.Message);
            }
        }
    }
}
