
---------------------------------------------------------------------------
--1. Criacao das tabelas
---------------------------------------------------------------------------

-- Create LMS database
CREATE DATABASE LMS;

-- Use LMS database 
USE LMS;

---------------------------------------------------------------------------
--2. Criacao das tabelas
---------------------------------------------------------------------------

-- Create User table
CREATE TABLE [dbo].[User]
(
    [UserId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [TypeId] INT NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [RegistrationDate] DATETIME NOT NULL,
    [Password] NVARCHAR(255) NOT NULL
);

-- Create Author table
CREATE TABLE [dbo].[Author]
(
    [AuthorId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Country] NVARCHAR(50) NOT NULL,
    [Birthdate] DATETIME NOT NULL
);

-- Create Category table
CREATE TABLE [dbo].[Category]
(
    [CategoryId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Description] NVARCHAR(100) NOT NULL
);

-- Create Book table
CREATE TABLE [dbo].[Book]
(
    [BookId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [AuthorId] INT NOT NULL,
    [CategoryId] INT NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [ISBN] NVARCHAR(50) NOT NULL,
    [PublishedYear] INT NOT NULL,
    [Image] VARBINARY(MAX) NULL,
    CONSTRAINT FK_Book_Author FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Author]([AuthorId]),
    CONSTRAINT FK_Book_Category FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category]([CategoryId])
);

-- Create Loan table
CREATE TABLE [dbo].[Loan]
(
    [LoanId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [BookId] INT NOT NULL,
    [LoanDate] DATETIME NOT NULL,
    [DueDate] DATETIME NOT NULL,
    [ReturnDate] DATETIME NULL,
    CONSTRAINT FK_Loan_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId]),
    CONSTRAINT FK_Loan_Book FOREIGN KEY ([BookId]) REFERENCES [dbo].[Book]([BookId])
);

---------------------------------------------------------------------------
--3. Criacao das Stored Procedures de Listagem
---------------------------------------------------------------------------

-- Create spListagem_User
CREATE OR ALTER PROCEDURE [dbo].[spListagem_User](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT * FROM [User]
    ORDER BY CASE WHEN @ordem IS NULL THEN UserId ELSE @ordem END;
END

-- Create spListagem_Author
CREATE OR ALTER PROCEDURE [dbo].[spListagem_Author](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT * FROM [Author]
    ORDER BY CASE WHEN @ordem IS NULL THEN AuthorId ELSE @ordem END;
END

-- Create spListagem_Category
CREATE OR ALTER PROCEDURE [dbo].[spListagem_Category](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT * FROM [Category]
    ORDER BY CASE WHEN @ordem IS NULL THEN CategoryId ELSE @ordem END;
END

-- Create spListagem_Book
CREATE OR ALTER PROCEDURE [dbo].[spListagem_Book](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT [Book].*, [Author].Name as 'AuthorName', [Category].Description as 'CategoryDescription'
    FROM Book
    INNER JOIN [Author] ON [Author].AuthorId = [Book].AuthorId
    INNER JOIN [Category] ON [Category].CategoryId = [Book].CategoryId
    ORDER BY CASE WHEN @ordem IS NULL THEN BookId ELSE @ordem END;
END

-- Create spListagem_Loan
CREATE OR ALTER PROCEDURE [dbo].[spListagem_Loan](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT [Loan].*, [User].Name as 'UserName', [Book].Title as 'BookTitle'
    FROM [Loan]
    INNER JOIN [User] ON [User].UserId = [Loan].UserId
    INNER JOIN [Book] ON [Book].BookId = [Loan].BookId
    ORDER BY CASE WHEN @ordem IS NULL THEN LoanId ELSE @ordem END;
END

---------------------------------------------------------------------------
--4. Criacao das Stored Procedures genericas
---------------------------------------------------------------------------

-- Create spDelete
CREATE OR ALTER PROCEDURE [dbo].[spDelete] 
    @id INT,
    @tabela NVARCHAR(50)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    DECLARE @primaryKey NVARCHAR(50);

    -- Assume que a coluna de chave primaria segue o padrao <NomeDaTabela>Id
    SET @primaryKey = @tabela + 'Id';

    -- Constr�i a query dinamicamente
    SET @sql = 'DELETE FROM [' + @tabela + '] WHERE ' + @primaryKey + ' = @id';

    -- Executa a query dinamica com o parametro @id
    EXEC sp_executesql @sql, N'@id INT', @id;
END

-- Create spConsulta
CREATE OR ALTER PROCEDURE [dbo].[spConsulta] 
    @id INT,
    @tabela NVARCHAR(50)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    DECLARE @primaryKey NVARCHAR(50);

    -- Assume que a coluna de chave primaria segue o padrao <NomeDaTabela>Id
    SET @primaryKey = @tabela + 'Id';

    -- Constroi a query dinamicamente
    SET @sql = 'SELECT * FROM [' + @tabela + '] WHERE ' + @primaryKey + ' = @id';

    -- Executa a query dinamica com o parametro @id
    EXEC sp_executesql @sql, N'@id INT', @id;
END

---------------------------------------------------------------------------
--5. Criacao das Stored Procedures de CRUD da tabela User
---------------------------------------------------------------------------

-- Create spInsert_User
CREATE OR ALTER PROCEDURE [dbo].[spInsert_User] 
    @TypeId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @RegistrationDate DATETIME,
    @Password NVARCHAR(255)
AS
BEGIN
    INSERT INTO [dbo].[User] (TypeId, Name, Email, RegistrationDate, Password)
    VALUES (@TypeId, @Name, @Email, @RegistrationDate, @Password);
END

-- Create spUpdate_User
CREATE OR ALTER PROCEDURE [dbo].[spUpdate_User] 
    @UserId INT,
    @TypeId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @RegistrationDate DATETIME,
    @Password NVARCHAR(255)
AS
BEGIN
    UPDATE [dbo].[User]
    SET TypeId = @TypeId,
        Name = @Name,
        Email = @Email,
        RegistrationDate = @RegistrationDate,
        Password = @Password
    WHERE UserId = @UserId;
END


---------------------------------------------------------------------------
--6. Criacao das Stored Procedures de CRUD da tabela Author
---------------------------------------------------------------------------

-- Create spInsert_Author
CREATE OR ALTER PROCEDURE [dbo].[spInsert_Author] 
    @Name NVARCHAR(100),
    @Country NVARCHAR(50),
    @Birthdate DATETIME
AS
BEGIN
    INSERT INTO [dbo].[Author] (Name, Country, Birthdate)
    VALUES (@Name, @Country, @Birthdate);
END

-- Create spUpdate_Author
CREATE OR ALTER PROCEDURE [dbo].[spUpdate_Author] 
    @AuthorId INT,
    @Name NVARCHAR(100),
    @Country NVARCHAR(50),
    @Birthdate DATETIME
AS
BEGIN
    UPDATE [dbo].[Author]
    SET Name = @Name,
        Country = @Country,
        Birthdate = @Birthdate
    WHERE AuthorId = @AuthorId;
END

---------------------------------------------------------------------------
--7. Criacao das Stored Procedures de CRUD da tabela Category
---------------------------------------------------------------------------

-- Create spInsert_Category
CREATE OR ALTER PROCEDURE [dbo].[spInsert_Category] 
    @Description NVARCHAR(100)
AS
BEGIN
    INSERT INTO [dbo].[Category] (Description)
    VALUES (@Description);
END

-- Create spUpdate_Category
CREATE OR ALTER PROCEDURE [dbo].[spUpdate_Category] 
    @CategoryId INT,
    @Description NVARCHAR(100)
AS
BEGIN
    UPDATE [dbo].[Category]
    SET Description = @Description
    WHERE CategoryId = @CategoryId;
END

---------------------------------------------------------------------------
--8. Criacao das Stored Procedures de CRUD da tabela Book
---------------------------------------------------------------------------

-- Create spInsert_Book
CREATE OR ALTER PROCEDURE [dbo].[spInsert_Book] 
    @AuthorId INT,
    @CategoryId INT,
    @Title NVARCHAR(200),
    @ISBN NVARCHAR(50),
    @PublishedYear INT,
    @Image VARBINARY(MAX) = NULL
AS
BEGIN
    INSERT INTO [dbo].[Book] (AuthorId, CategoryId, Title, ISBN, PublishedYear, Image)
    VALUES (@AuthorId, @CategoryId, @Title, @ISBN, @PublishedYear, @Image);
END

-- Create spUpdate_Book
CREATE OR ALTER PROCEDURE [dbo].[spUpdate_Book] 
    @BookId INT,
    @AuthorId INT,
    @CategoryId INT,
    @Title NVARCHAR(200),
    @ISBN NVARCHAR(50),
    @PublishedYear INT,
    @Image VARBINARY(MAX) = NULL
AS
BEGIN
    UPDATE [dbo].[Book]
    SET AuthorId = @AuthorId,
        CategoryId = @CategoryId,
        Title = @Title,
        ISBN = @ISBN,
        PublishedYear = @PublishedYear,
        Image = @Image
    WHERE BookId = @BookId;
END

---------------------------------------------------------------------------
--9. Criacao das Stored Procedures de CRUD da tabela Loan
---------------------------------------------------------------------------

-- Create spInsert_Loan
CREATE OR ALTER PROCEDURE [dbo].[spInsert_Loan] 
    @UserId INT,
    @BookId INT,
    @LoanDate DATETIME,
    @DueDate DATETIME,
    @ReturnDate DATETIME = NULL
AS
BEGIN
    INSERT INTO [dbo].[Loan] (UserId, BookId, LoanDate, DueDate, ReturnDate)
    VALUES (@UserId, @BookId, @LoanDate, @DueDate, @ReturnDate);
END

-- Create spUpdate_Loan
CREATE OR ALTER PROCEDURE [dbo].[spUpdate_Loan] 
    @LoanId INT,
    @UserId INT,
    @BookId INT,
    @LoanDate DATETIME,
    @DueDate DATETIME,
    @ReturnDate DATETIME = NULL
AS
BEGIN
    UPDATE [dbo].[Loan]
    SET UserId = @UserId,
        BookId = @BookId,
        LoanDate = @LoanDate,
        DueDate = @DueDate,
        ReturnDate = @ReturnDate
    WHERE LoanId = @LoanId;
END
