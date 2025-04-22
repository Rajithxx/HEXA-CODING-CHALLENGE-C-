CREATE SCHEMA LoanSystem;
GO

CREATE TABLE LoanSystem.Customer (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,  
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(15) NOT NULL,
    CreditScore INT NOT NULL 
);
GO


CREATE TABLE LoanSystem.Loan (
    LoanId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    PrincipalAmount FLOAT NOT NULL,
    InterestRate FLOAT NOT NULL,
    LoanTerm INT NOT NULL,
    LoanStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    LoanType NVARCHAR(50) NOT NULL,

	PropertyAddress NVARCHAR(255) NULL,
    PropertyValue FLOAT NULL,
    CarModel NVARCHAR(100) NULL,
    CarValue FLOAT NULL,
	FOREIGN KEY (CustomerId) REFERENCES LoanSystem.Customer(CustomerId)
);
GO


IF OBJECT_ID('LoanSystem.Loan', 'U') IS NOT NULL
BEGIN
    DROP TABLE LoanSystem.Loan;
    PRINT 'Loan table dropped.';
END


IF OBJECT_ID('LoanSystem.Customer', 'U') IS NOT NULL
BEGIN
    DROP TABLE LoanSystem.Customer;
    PRINT 'Customer table dropped.';
END
GO


SELECT * FROM LoanSystem.Customer;



