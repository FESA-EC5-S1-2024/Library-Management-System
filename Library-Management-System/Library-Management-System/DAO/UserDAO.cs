using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Library_Management_System.Models;

namespace Library_Management_System.DAO {
    public class UserDAO : PadraoDAO<UserViewModel> {
        protected override SqlParameter[] CriaParametros(UserViewModel user, bool isInsert = false) {
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!isInsert) {
                parametros.Add(new SqlParameter("@UserId", user.Id));
            }

            parametros.Add(new SqlParameter("@TypeId", user.TypeId));
            parametros.Add(new SqlParameter("@Name", user.Name));
            parametros.Add(new SqlParameter("@Email", user.Email));
            parametros.Add(new SqlParameter("@RegistrationDate", user.RegistrationDate));
            parametros.Add(new SqlParameter("@Password", user.Password));

            return parametros.ToArray();
        }

        protected override UserViewModel MontaModel(DataRow registro) {
            UserViewModel user = new UserViewModel();
            user.Id = Convert.ToInt32(registro["UserId"]);
            user.TypeId = Convert.ToInt32(registro["TypeId"]);
            user.Name = registro["Name"].ToString();
            user.Email = registro["Email"].ToString();
            user.RegistrationDate = Convert.ToDateTime(registro["RegistrationDate"]);
            user.Password = registro["Password"].ToString();
            return user;
        }

        protected override void SetTabela() {
            Tabela = "User";
            NomeSpListagem = "spListagem_User"; // Você pode ter uma stored procedure específica para listagem, se necessário
        }
    }
}
