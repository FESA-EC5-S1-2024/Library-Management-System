
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

-- Create spConsulta_User
CREATE OR ALTER PROCEDURE [dbo].[spConsulta_User] 
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [User] WHERE Email = @Email
END

-- Create spDelete_User_And_Loans
CREATE OR ALTER PROCEDURE [dbo].[spDelete_User_And_Loans]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Deletar empréstimos relacionados ao usuário
        DELETE FROM Loan
        WHERE UserId = @id;

        -- Deletar o usuário
        DELETE FROM [User]
        WHERE UserId = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

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

-- Create spDelete_Author_And_Books
CREATE OR ALTER PROCEDURE [dbo].[spDelete_Author_And_Books]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Deletar empréstimos relacionados aos livros do autor
        DELETE FROM Loan
        WHERE BookId IN (SELECT BookId FROM Book WHERE AuthorId = @id);

        -- Deletar livros relacionados ao autor
        DELETE FROM Book
        WHERE AuthorId = @id;

        -- Deletar o autor
        DELETE FROM Author
        WHERE AuthorId = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

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

-- Create spDelete_Category_And_Books
CREATE OR ALTER PROCEDURE [dbo].[spDelete_Category_And_Books]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Deletar empréstimos relacionados aos livros da categoria
        DELETE FROM Loan
        WHERE BookId IN (SELECT BookId FROM Book WHERE CategoryId = @id);

        -- Deletar livros relacionados à categoria
        DELETE FROM Book
        WHERE CategoryId = @id;

        -- Deletar a categoria
        DELETE FROM Category
        WHERE CategoryId = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

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

-- Create spConsultaAvancada_Book
CREATE OR ALTER PROCEDURE [dbo].[spConsultaAvancada_Book]
    @descricao VARCHAR(MAX),
    @autor INT,
    @categoria INT,
    @dataInicial INT,
    @dataFinal INT
AS
BEGIN
    DECLARE @categIni INT
    DECLARE @categFim INT
    DECLARE @autorIni INT
    DECLARE @autorFim INT
    SET @categIni = CASE @categoria WHEN 0 THEN 0 ELSE @categoria END
    SET @categFim = CASE @categoria WHEN 0 THEN 999999 ELSE @categoria END
    SET @autorIni = CASE @autor WHEN 0 THEN 0 ELSE @autor END
    SET @autorFim = CASE @autor WHEN 0 THEN 999999 ELSE @autor END

    SELECT 
        Book.*, Category.Description AS 'CategoryDescription', Author.Name AS 'AuthorName'
    FROM Book
    INNER JOIN Category ON Book.categoryId = Category.CategoryId
    INNER JOIN Author ON Book.authorId = Author.AuthorId
    WHERE 
        Book.title LIKE '%' + @descricao + '%' AND
        Book.publishedYear BETWEEN @dataInicial AND @dataFinal AND
        Book.CategoryId BETWEEN @categIni AND @categFim AND
        Book.AuthorId BETWEEN @autorIni AND @autorFim;
END

-- Create spDelete_Book_And_Loans
CREATE OR ALTER PROCEDURE [dbo].[spDelete_Book_And_Loans]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Deletar empréstimos relacionados ao livro
        DELETE FROM Loan
        WHERE BookId = @id;

        -- Deletar o livro
        DELETE FROM Book
        WHERE BookId = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

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

-- Create spConsulta_Loan
CREATE OR ALTER PROCEDURE [dbo].[spConsulta_Loan] 
    @UserId INT
AS
BEGIN
	SELECT [Loan].*, [User].Name as 'UserName', [Book].Title as 'BookTitle'
    FROM [Loan]
    INNER JOIN [User] ON [User].UserId = [Loan].UserId
    INNER JOIN [Book] ON [Book].BookId = [Loan].BookId
	WHERE [Loan].UserId = @UserId
END

-- Create spConsultaAvancada_Loan
CREATE OR ALTER PROCEDURE [dbo].[spConsultaAvancada_Loan]
    @usuario VARCHAR(MAX),
    @descricao VARCHAR(MAX),
    @dataInicial DATETIME,
    @dataFinal DATETIME
AS
BEGIN
    SELECT 
        [Loan].*, [User].Name AS 'UserName', [Book].Title AS 'BookTitle'
    FROM [Loan]
    INNER JOIN [User] ON [Loan].UserId = [User].UserId
    INNER JOIN [Book] ON [Loan].BookId = [Book].BookId
    WHERE 
        [User].Name LIKE '%' + @usuario + '%' AND
        [Book].Title LIKE '%' + @descricao + '%' AND
        Book.publishedYear BETWEEN @dataInicial AND @dataFinal;
END


---------------------------------------------------------------------------
--10. Criacao do sa do sistema
---------------------------------------------------------------------------

INSERT INTO [dbo].[User] (TypeId, Name, Email, RegistrationDate, Password)
VALUES (1, 'System Admin', 'sa', GETDATE(), '1234');
