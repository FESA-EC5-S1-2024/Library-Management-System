using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class BookDAO : PadraoDAO<BookViewModel> {
        protected override SqlParameter[] CriaParametros(BookViewModel book, bool isInsert = false) {
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!isInsert) {
                parametros.Add(new SqlParameter("@UserId", book.Id));
            }

            parametros.Add(new SqlParameter ("AuthorId", book.AuthorId));
            parametros.Add(new SqlParameter("CategoryId", book.CategoryId));
            parametros.Add(new SqlParameter("Title", book.Title));
            parametros.Add(new SqlParameter("ISBN", book.ISBN));
            parametros.Add(new SqlParameter("PublishedYear", book.PublishedYear));
            parametros.Add(new SqlParameter("Image", HelperDAO.NullAsDbNull(book.Image)));
            return parametros.ToArray();
        }

        protected override BookViewModel MontaModel(DataRow registro) {
            BookViewModel b = new BookViewModel();
            b.Id = Convert.ToInt32(registro["BookId"]);
            b.AuthorId = Convert.ToInt32(registro["AuthorId"]);
            b.CategoryId = Convert.ToInt32(registro["CategoryId"]);
            b.Title = registro["Title"].ToString();
            b.ISBN = registro["ISBN"].ToString();
            b.PublishedYear = Convert.ToInt32(registro["PublishedYear"]);
            b.Image = registro["Image"] as byte[];
            return b;
        }

        protected override void SetTabela() {
            Tabela = "Book";
            NomeSpListagem = "spListagem_Book";
        }
    }
}
