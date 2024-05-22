using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class CategoryDAO : PadraoDAO<CategoryViewModel> {
        protected override SqlParameter[] CriaParametros(CategoryViewModel category, bool isInsert = false) {
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!isInsert) {
                parametros.Add(new SqlParameter("CategoryId", category.Id));
            }

            parametros.Add(new SqlParameter("Description", category.Description));
            return parametros.ToArray();
        }

        protected override CategoryViewModel MontaModel(DataRow registro) {
            CategoryViewModel c = new CategoryViewModel();
            c.Id = Convert.ToInt32(registro["CategoryId"]);
            c.Description = registro["Description"].ToString();
            return c;
        }

        protected override void SetTabela() {
            Tabela = "Category";
            NomeSpListagem = "spListagem_Category";
            NomeSpDelete = "spDelete_Category_And_Books";
        }
    }
}