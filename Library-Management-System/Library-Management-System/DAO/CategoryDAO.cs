using Library_Management_System.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class CategoryDAO : PadraoDAO<CategoryViewModel> {
        protected override SqlParameter[] CriaParametros(CategoryViewModel category) {
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("CategoryId", category.Id);
            p[1] = new SqlParameter("Description", category.Description);
            return p;
        }

        protected override CategoryViewModel MontaModel(DataRow registro) {
            CategoryViewModel c = new CategoryViewModel();
            c.Id = Convert.ToInt32(registro["CategoryId"]);
            c.Description = registro["Description"].ToString();
            return c;
        }

        protected override void SetTabela() {
            Tabela = "Category";
            NomeSpListagem = "spListagemCategory";
        }
    }
}