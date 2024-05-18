using Library_Management_System.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class AuthorDAO : PadraoDAO<AuthorViewModel> {
        protected override SqlParameter[] CriaParametros(AuthorViewModel author) {
            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("AuthorId", author.Id);
            p[1] = new SqlParameter("Name", author.Name);
            p[2] = new SqlParameter("Country", author.Country);
            p[3] = new SqlParameter("Birthdate", author.Birthdate);
            return p;
        }

        protected override AuthorViewModel MontaModel(DataRow registro) {
            AuthorViewModel a = new AuthorViewModel();
            a.Id = Convert.ToInt32(registro["AuthorId"]);
            a.Name = registro["Name"].ToString();
            a.Country = registro["Country"].ToString();
            a.Birthdate = Convert.ToDateTime(registro["Birthdate"]);
            return a;
        }

        protected override void SetTabela() {
            Tabela = "Author";
            NomeSpListagem = "spListagemAuthor";
        }
    }
}
