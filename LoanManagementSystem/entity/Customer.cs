using System;
using LoanManagementSystem.util;
using Microsoft.Data.SqlClient;

namespace LoanManagementSystem.entity
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CreditScore { get; set; }

    
        public Customer() { }

      
        public Customer(int customerId, string name, string email, string phoneNumber, string address, int creditScore)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            CreditScore = creditScore;
        }

   
        public void DisplayCustomerInfo()
        {
            Console.WriteLine($"Customer ID: {CustomerId}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber}, Address: {Address}, Credit Score: {CreditScore}");
        }

      
        public static Customer CreateCustomer()
        {
            Console.WriteLine("Enter the customer's name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter the customer's address:");
            string address = Console.ReadLine();

            Console.WriteLine("Enter the customer's phone number:");
            string phoneNumber = Console.ReadLine();

            Console.WriteLine("Enter the customer's email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter the customer's credit score:");
            int creditScore = 0;
            while (!int.TryParse(Console.ReadLine(), out creditScore) || creditScore < 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid credit score (positive integer):");
            }

           
            int customerId = 0;

            
            return new Customer(customerId, name, email, phoneNumber, address, creditScore);
        }

       
        public void InsertCustomer()
        {
            using (var connection = DbUtil.GetConnection())
            {
                var command = new SqlCommand("INSERT INTO LoanSystem.Customer (Name, Address, Email, PhoneNumber, CreditScore) VALUES (@Name, @Address, @Email, @PhoneNumber, @CreditScore);", connection);

                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                command.Parameters.AddWithValue("@CreditScore", CreditScore);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Customer inserted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting customer: {ex.Message}");
                }
            }
        }
    }
}
