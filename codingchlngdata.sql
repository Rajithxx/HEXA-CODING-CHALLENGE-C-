
INSERT INTO LoanSystem.Customer (Name, Address, Email, PhoneNumber, CreditScore)
VALUES
('John Doe', '123 Elm Street, Springfield', 'john.doe@example.com', '555-1234', 720),
('Jane Smith', '456 Oak Avenue, Rivertown', 'jane.smith@example.com', '555-5678', 680),
('Alice Johnson', '789 Pine Road, Lakedale', 'alice.johnson@example.com', '555-9876', 750),
('Bob Brown', '101 Maple Lane, Hillcrest', 'bob.brown@example.com', '555-3456', 690),
('Charlie White', '202 Birch Boulevard, Woodlands', 'charlie.white@example.com', '555-6789', 710);
GO


INSERT INTO LoanSystem.Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanStatus, LoanType, PropertyAddress, PropertyValue, CarModel, CarValue)
VALUES
(1, 250000, 6.5, 240, 'Pending', 'HomeLoan', '123 Elm Street, Springfield', 300000, NULL, NULL),
(2, 20000, 7.5, 60, 'Approved', 'CarLoan', NULL, NULL, 'Toyota Camry', 22000),
(3, 150000, 5.0, 180, 'Pending', 'HomeLoan', '789 Pine Road, Lakedale', 180000, NULL, NULL),
(4, 15000, 8.0, 36, 'Approved', 'CarLoan', NULL, NULL, 'Honda Accord', 18000),
(5, 350000, 6.0, 240, 'Approved', 'HomeLoan', '202 Birch Boulevard, Woodlands', 400000, NULL, NULL);
GO
