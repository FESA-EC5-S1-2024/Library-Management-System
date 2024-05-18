
---------------------------------------------------------------------------
--1. Cria��o das tabelas
---------------------------------------------------------------------------

-- Create LMS database
CREATE DATABASE LMS;

-- Use LMS database 
USE LMS;

---------------------------------------------------------------------------
--2. Cria��o das tabelas
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
--3. Cria��o das Stored Procedures de Listagem
---------------------------------------------------------------------------

-- Create spListagemUser
CREATE OR ALTER PROCEDURE [dbo].[spListagemUser](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT * FROM [User]
    ORDER BY CASE WHEN @ordem IS NULL THEN UserId ELSE @ordem END;
END

-- Create spListagemAuthor
CREATE OR ALTER PROCEDURE [dbo].[spListagemAuthor](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT * FROM [Author]
    ORDER BY CASE WHEN @ordem IS NULL THEN AuthorId ELSE @ordem END;
END

-- Create spListagemCategory
CREATE OR ALTER PROCEDURE [dbo].[spListagemCategory](
   @tabela varchar(max),
   @ordem varchar(max)
)
AS
BEGIN
    SELECT * FROM [Category]
    ORDER BY CASE WHEN @ordem IS NULL THEN CategoryId ELSE @ordem END;
END

-- Create spListagemBook
CREATE OR ALTER PROCEDURE [dbo].[spListagemBook](
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

-- Create spListagemLoan
CREATE OR ALTER PROCEDURE [dbo].[spListagemLoan](
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
--4. Cria��o das Stored Procedures gen�ricas
---------------------------------------------------------------------------

-- Create spDelete
CREATE OR ALTER PROCEDURE [dbo].[spDelete] 
    @id INT,
    @tabela NVARCHAR(50)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    DECLARE @primaryKey NVARCHAR(50);

    -- Assume que a coluna de chave prim�ria segue o padr�o <NomeDaTabela>Id
    SET @primaryKey = @tabela + 'Id';

    -- Constr�i a query dinamicamente
    SET @sql = 'DELETE FROM ' + @tabela + ' WHERE ' + @primaryKey + ' = @id';

    -- Executa a query din�mica com o par�metro @id
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

    -- Assume que a coluna de chave prim�ria segue o padr�o <NomeDaTabela>Id
    SET @primaryKey = @tabela + 'Id';

    -- Constr�i a query dinamicamente
    SET @sql = 'SELECT * FROM ' + @tabela + ' WHERE ' + @primaryKey + ' = @id';

    -- Executa a query din�mica com o par�metro @id
    EXEC sp_executesql @sql, N'@id INT', @id;
END

-- Create spProximoId
CREATE OR ALTER PROCEDURE [dbo].[spProximoId]
    @tabela NVARCHAR(50)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    DECLARE @primaryKey NVARCHAR(MAX);

    -- Assume que a coluna de chave prim�ria segue o padr�o <NomeDaTabela>Id
    SET @primaryKey = @tabela + 'Id';

    -- Constr�i a query dinamicamente
    SET @sql = 'SELECT ISNULL(MAX(' + @primaryKey + '), 0) + 1 AS ProximoId FROM ' + @tabela;

    -- Executa a query din�mica
    EXEC sp_executesql @sql;
END

---------------------------------------------------------------------------
--5. Cria��o das Stored Procedures de CRUD da tabela User
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

    SELECT SCOPE_IDENTITY() AS [UserId]; -- retorna o ID do registro inserido
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
--6. Cria��o das Stored Procedures de CRUD da tabela Author
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

    SELECT SCOPE_IDENTITY() AS [AuthorId]; -- retorna o ID do registro inserido
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
--7. Cria��o das Stored Procedures de CRUD da tabela Category
---------------------------------------------------------------------------

-- Create spInsert_Category
CREATE OR ALTER PROCEDURE [dbo].[spInsert_Category] 
    @Description NVARCHAR(100)
AS
BEGIN
    INSERT INTO [dbo].[Category] (Description)
    VALUES (@Description);

    SELECT SCOPE_IDENTITY() AS [CategoryId]; -- retorna o ID do registro inserido
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
--8. Cria��o das Stored Procedures de CRUD da tabela Book
---------------------------------------------------------------------------

-- Create spInsert_Book
CREATE PROCEDURE [dbo].[spInsert_Book] 
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

    SELECT SCOPE_IDENTITY() AS [BookId]; -- retorna o ID do registro inserido
END

-- Create spUpdate_Book
CREATE PROCEDURE [dbo].[spUpdate_Book] 
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
--9. Cria��o das Stored Procedures de CRUD da tabela Loan
---------------------------------------------------------------------------

-- Create spInsert_Loan
CREATE PROCEDURE [dbo].[spInsert_Loan] 
    @UserId INT,
    @BookId INT,
    @LoanDate DATETIME,
    @DueDate DATETIME,
    @ReturnDate DATETIME = NULL
AS
BEGIN
    INSERT INTO [dbo].[Loan] (UserId, BookId, LoanDate, DueDate, ReturnDate)
    VALUES (@UserId, @BookId, @LoanDate, @DueDate, @ReturnDate);

    SELECT SCOPE_IDENTITY() AS [LoanId]; -- retorna o ID do registro inserido
END

-- Create spUpdate_Loan
CREATE PROCEDURE [dbo].[spUpdate_Loan] 
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