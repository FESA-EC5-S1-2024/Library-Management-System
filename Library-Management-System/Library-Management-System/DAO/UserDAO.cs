using Library_Management_System.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO
{
    class UserDAO : PadraoDAO<UserViewModel> {
        protected override SqlParameter[] CriaParametros(UserViewModel user) {
            SqlParameter[] p = new SqlParameter[6];
            p[0] = new SqlParameter("UserId", user.Id);
            p[1] = new SqlParameter("TypeId", user.TypeId);
            p[2] = new SqlParameter("Name", user.Name);
            p[3] = new SqlParameter("Email", user.Email);
            p[4] = new SqlParameter("RegistrationDate", user.RegistrationDate);
            p[5] = new SqlParameter("Password", user.Password);
            return p;
        }

        protected override UserViewModel MontaModel(DataRow registro) {
            UserViewModel u = new UserViewModel();
            u.Id = Convert.ToInt32(registro["UserId"]);
            u.TypeId = Convert.ToInt32(registro["TypeId"]);
            u.Name = registro["Name"].ToString();
            u.Email = registro["Email"].ToString();
            u.RegistrationDate = Convert.ToDateTime(registro["RegistrationDate"]);
            u.Password = registro["Password"].ToString();
            return u;
        }

        protected override void SetTabela() {
            Tabela = "User";
            NomeSpListagem = "spListagemUser";
        }
    }
}