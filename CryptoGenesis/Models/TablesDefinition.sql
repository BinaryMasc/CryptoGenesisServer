---------------------------------
--	Tables and Database Creation
---------------------------------
USE master;
CREATE DATABASE CryptoGenesis;

BEGIN TRANSACTION
	
	USE CryptoGenesis;

	CREATE TABLE ConfigReward(
		CurrStandarRewardPerAdTether DECIMAL(15,10) NOT NULL DEFAULT 0,
		CurrStandarRewardPerAdDalion DECIMAL(15,10) NOT NULL DEFAULT 0,
		MinTimeStandarReward INT NOT NULL DEFAULT 30,	 --	Minutes
		MinWithdrawalAmountTether DECIMAL(15,10) NOT NULL DEFAULT 10,
		MinWithdrawalAmountDalion DECIMAL(15,10) NOT NULL DEFAULT 10,
		RewardByReferredPercent FLOAT NOT NULL DEFAULT 25
	);

	CREATE TABLE ConfigGeneral(
		AllowedVersion VARCHAR(10) NOT NULL,
		MaintenanceMode BIT NOT NULL DEFAULT 0
	);

	CREATE TABLE [User](
		UserId INT NOT NULL IDENTITY(1,1),
		Username VARCHAR(16) NOT NULL,
		Email VARCHAR(32) NOT NULL,
		[Password] VARCHAR(96) NOT NULL,
		UserType INT NOT NULL DEFAULT 0,
		ReferredCode VARCHAR(10) NULL,
		AffiliatedBy INT NULL,
		EmailVerified BIT NOT NULL DEFAULT 0,
		[Enabled] BIT NOT NULL DEFAULT 1,
		PRIMARY KEY (UserId),

		FOREIGN KEY (AffiliatedBy) REFERENCES [User](UserId)
	);

	CREATE TABLE Wallet(
		WalletId INT NOT NULL IDENTITY(1,1),
		UserId INT NOT NULL,
		CurrencyId VARCHAR(5),
		Balance DECIMAL(15,10) NOT NULL DEFAULT 0,
		FreezeBalance DECIMAL(15,10) NOT NULL DEFAULT 0,
		Credit DECIMAL(15,10) NOT NULL DEFAULT 0,
		BlockChainAddress VARCHAR NULL,
		PRIMARY KEY (WalletId),
	
		FOREIGN KEY (UserId) REFERENCES [User](UserId)
	);

	CREATE TABLE Withdrawal(
		WithdrawalId INT NOT NULL IDENTITY(1,1),
		WalletId INT NOT NULL,
		CurrencyId VARCHAR(5) NOT NULL,
		Amount DECIMAL(15,10) NOT NULL,
		[Date] DATETIME NOT NULL,
		PRIMARY KEY (WithdrawalId),

		FOREIGN KEY (WalletId) REFERENCES Wallet(WalletId)
	);

	CREATE TABLE SessionToken(
		SessionTokenId INT NOT NULL IDENTITY (1,1),
		UserId INT,
		TokenString VARCHAR(96) NOT NULL,
		CreationDate Datetime NOT NULL,
		PRIMARY KEY (SessionTokenId),
	
		FOREIGN KEY (UserId) REFERENCES [User](UserId)
	);


	CREATE TABLE AdProcessType(
		AdProcessTypeId INT NOT NULL IDENTITY (1,1),
		AdProcessTypeName VARCHAR(30),
		Status INT NOT NULL DEFAULT 0,

		PRIMARY KEY (AdProcessTypeId)
	);

	CREATE TABLE AdProcessTypeDetail(
		AdProcessTypeDetailId INT NOT NULL IDENTITY (1,1),
		AdProcessTypeDetailName VARCHAR(30),
		Status INT NOT NULL DEFAULT 0,

		PRIMARY KEY (AdProcessTypeDetailId)
	);


	CREATE TABLE AdProcess(
		AdProcessId BIGINT NOT NULL IDENTITY (1,1),
		UserId INT NOT NULL,
		AdProcessTypeId INT NOT NULL,
		Date DATETIME NOT NULL,
		PRIMARY KEY (AdProcessId),

		FOREIGN KEY (UserId) REFERENCES [User](UserId),
		FOREIGN KEY (AdProcessTypeId) REFERENCES AdProcessType(AdProcessTypeId)
	);
	CREATE TABLE AdProcessDetail(
		RowId INT NOT NULL,
		AdProcessId BIGINT NOT NULL,
		AdProcessTypeDetailId INT NOT NULL,
		Amount DECIMAL(15,10) NULL,
		Date DATETIME NOT NULL,
		--PRIMARY KEY (RowId),

		FOREIGN KEY (AdProcessId) REFERENCES AdProcess(AdProcessId),
		FOREIGN KEY (AdProcessTypeDetailId) REFERENCES AdProcessTypeDetail(AdProcessTypeDetailId)
	);

COMMIT