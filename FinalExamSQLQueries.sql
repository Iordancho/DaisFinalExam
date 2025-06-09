CREATE DATABASE OnlinePayments2

USE OnlinePayments2

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(256) NOT NULL
);

CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY IDENTITY,
	AccountName NVARCHAR(100) NOT NULL,
    AccountNumber NVARCHAR(22) NOT NULL UNIQUE
);


CREATE TABLE UserAccounts (
    UserId INT NOT NULL,
    AccountId INT NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (UserId, AccountId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId)
);

CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY,
	CreatorId INT NOT NULL,
    FromAccountId INT NOT NULL,
    ToAccountNumber NVARCHAR(22) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Reason NVARCHAR(32) NOT NULL,
    [Status] NVARCHAR(15) NOT NULL,
    CreatedAt DATETIME NOT NULL,
	FOREIGN KEY (CreatorId) REFERENCES Users(UserId),
    FOREIGN KEY (FromAccountId) REFERENCES Accounts(AccountId)
);
INSERT INTO Accounts (AccountName, AccountNumber)
VALUES 
    (N'Разплащателна', 'BG11AAAA22223333444455'),
    (N'Спестовна', 'BG22BBBB33334444555566'),
    (N'Инвестиционна', 'BG33CCCC44445555666677');

	INSERT INTO Users (FullName, Username, [Password])
VALUES 
('Ivan Petrov', 'ivanp', 'A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA'),
('Maria Georgieva', 'mgeorgieva', 'A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA'),
('Georgi Ivanov', 'givanov', 'A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA'),
('Elena Dimitrova', 'edimitrova', 'A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA'),
('Dimitar Nikolov', 'dnikolov', 'A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA');

USE OnlinePayments2

INSERT INTO UserAccounts (UserId, AccountId, Balance) VALUES (1, 1, 1000.00);
INSERT INTO UserAccounts (UserId, AccountId, Balance) VALUES (1, 2, 5000.00);
INSERT INTO UserAccounts (UserId, AccountId, Balance) VALUES (1, 3, 20000.00);

-- User 2 (Maria Dimitrova) has access to 2 accounts
INSERT INTO UserAccounts (UserId, AccountId, Balance) VALUES (2, 1, 250.00);
INSERT INTO UserAccounts (UserId, AccountId, Balance) VALUES (2, 2, 1200.00);

-- User 3 (Georgi Stoyanov) has access to 1 account
INSERT INTO UserAccounts (UserId, AccountId, Balance) VALUES (3, 3, 800.00);