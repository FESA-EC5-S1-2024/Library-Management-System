using Library_Management_System.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class BookDAO : PadraoDAO<BookViewModel> {
        protected override SqlParameter[] CriaParametros(BookViewModel book) {
            SqlParameter[] p = new SqlParameter[8];
            p[0] = new SqlParameter("BookId", book.Id);
            p[1] = new SqlParameter("AuthorId", book.AuthorId);
            p[2] = new SqlParameter("CategoryId", book.CategoryId);
            p[3] = new SqlParameter("Title", book.Title);
            p[4] = new SqlParameter("ISBN", book.ISBN);
            p[5] = new SqlParameter("PublishedYear", book.PublishedYear);
            p[6] = new SqlParameter("Image", HelperDAO.NullAsDbNull(book.Image));
            return p;
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
            NomeSpListagem = "spListagemBook";
        }
    }
}
