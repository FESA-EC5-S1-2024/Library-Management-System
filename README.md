# Sistema de Gerenciamento de Biblioteca

Este sistema é projetado para gerenciar as operações de uma biblioteca, incluindo cadastro de usuários, livros, autores, categorias e empréstimos. Além disso, o sistema oferece funcionalidades para login, consulta avançada e controle de transações.

## Funcionalidades

1. **Página Inicial (Home)**
   - Apresenta uma visão geral das funcionalidades disponíveis.

2. **Cadastro de Usuários**
   - Permite o registro e gerenciamento de usuários.
   - Possui campos como ID, Tipo de Usuário, Nome, Email, Data de Registro e Senha.

3. **Cadastro de Livros**
   - Permite a inclusão, atualização, listagem e exclusão de registros de livros.
   - Cada livro possui campos como ID, Autor, Categoria, Título, ISBN, Ano de Publicação e Imagem.

4. **Cadastro de Autores**
   - Funcionalidade para gerenciar dados dos autores.
   - Inclui campos como ID, Nome, País e Data de Nascimento.

5. **Cadastro de Categorias**
   - Funcionalidade para gerenciar categorias dos livros.
   - Inclui campos como ID e Descrição.

6. **Empréstimos de Livros**
   - Regista e gerencia os empréstimos de livros.
   - Contém campos como ID, Usuário, Livro, Data de Empréstimo, Data de Devolução e Data de Retorno.

7. **Controle de Login**
   - Sistema de autenticação com login e senha.
   - Opção de cadastro para novos usuários e administração.

8. **Consultas Avançadas**
   - Consultas utilizando filtros e JOINs com múltiplos critérios como data, categoria, estado, cidade, etc.

9. **Controle de Transações**
   - Implementação de controle de transações nas operações críticas para garantir a integridade dos dados.

10. **Interfaces Gráficas**
    - Utilização de Tailwind CSS para estilização das páginas, com suporte a modo escuro e responsividade.

## Estrutura do Projeto

### Modelos (Models)
Os modelos representam as entidades do banco de dados e são utilizados na camada de negócio para manipular os dados.

- **UserViewModel**
- **BookViewModel**
- **AuthorViewModel**
- **CategoryViewModel**
- **LoanViewModel**
- **ErrorViewModel**

### Data Access Objects (DAOs)
Os DAOs encapsulam o acesso aos dados e executam operações no banco de dados utilizando Stored Procedures.

- **PadraoDAO<T>** (Classe base)
- **UserDAO**
- **BookDAO**
- **AuthorDAO**
- **CategoryDAO**
- **LoanDAO**

### Stored Procedures
As procedures são scripts armazenados no banco de dados que realizam operações como inserção, atualização, exclusão e consulta.

- **spListagem** (genérico para listagem)
- **spListagemUser**
- **spListagemBook**
- **spListagemAuthor**
- **spListagemCategory**
- **spListagemLoan**
- **spInsert_User**, **spUpdate_User**, **spDelete_User**
- **spInsert_Book**, **spUpdate_Book**, **spDelete_Book**
- **spInsert_Author**, **spUpdate_Author**, **spDelete_Author**
- **spInsert_Category**, **spUpdate_Category**, **spDelete_Category**
- **spInsert_Loan**, **spUpdate_Loan**, **spDelete_Loan**

## Como Executar o Projeto

### Pré-requisitos
- .NET Core SDK
- SQL Server
- Visual Studio ou Visual Studio Code

### Passos para Configuração

1. **Configurar o Banco de Dados**
   - Execute o script SQL fornecido (`database.sql`) no SQL Server para criar as tabelas e stored procedures.

2. **Configurar a String de Conexão**
   - No arquivo `appsettings.json`, configure a string de conexão para seu banco de dados SQL Server.

3. **Executar o Aplicativo**
   - Abra o projeto no Visual Studio ou Visual Studio Code.
   - Compile e execute o projeto.
   - Acesse `http://localhost:56271` no navegador.

### Usuários Administrativos
- Os detalhes de login para usuários administrativos podem ser configurados diretamente no banco de dados ou através da funcionalidade de cadastro de usuários.

## Considerações Finais

Este projeto visa demonstrar um sistema completo de gerenciamento de livraria utilizando boas práticas de programação, controle de transações, consultas avançadas e um design de front-end responsivo e moderno.
