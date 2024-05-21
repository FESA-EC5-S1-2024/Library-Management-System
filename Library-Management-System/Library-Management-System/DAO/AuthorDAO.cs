using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class AuthorDAO : PadraoDAO<AuthorViewModel> {
        protected override SqlParameter[] CriaParametros(AuthorViewModel author, bool isInsert = false) {
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!isInsert) {
                parametros.Add(new SqlParameter("AuthorId", author.Id));
            }

            parametros.Add(new SqlParameter("Name", author.Name));
            parametros.Add(new SqlParameter("Country", author.Country));
            parametros.Add(new SqlParameter("Birthdate", author.Birthdate));
            return parametros.ToArray();
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
            NomeSpListagem = "spListagem_Author";
        }
    }
}