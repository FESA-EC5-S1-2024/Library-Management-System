using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class LoanDAO : PadraoDAO<LoanViewModel> {
        protected override SqlParameter[] CriaParametros(LoanViewModel loan, bool isInsert = false) {
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!isInsert) {
                parametros.Add(new SqlParameter("@UserId", loan.Id));
            }

            parametros.Add(new SqlParameter("UserId", loan.UserId));
            parametros.Add(new SqlParameter("BookId", loan.BookId));
            parametros.Add(new SqlParameter("LoanDate", loan.LoanDate));
            parametros.Add(new SqlParameter("DueDate", loan.DueDate));
            parametros.Add(new SqlParameter("ReturnDate", HelperDAO.NullAsDbNull(loan.ReturnDate)));
            return parametros.ToArray();
        }

        protected override LoanViewModel MontaModel(DataRow registro) {
            LoanViewModel l = new LoanViewModel();
            l.Id = Convert.ToInt32(registro["LoanId"]);
            l.UserId = Convert.ToInt32(registro["UserId"]);
            l.BookId = Convert.ToInt32(registro["BookId"]);
            l.LoanDate = Convert.ToDateTime(registro["LoanDate"]);
            l.DueDate = Convert.ToDateTime(registro["DueDate"]);
            l.ReturnDate = registro["ReturnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(registro["ReturnDate"]);

            if (registro.Table.Columns.Contains("UserName"))
                l.UserName = registro["UserName"].ToString();
            if (registro.Table.Columns.Contains("BookTitle"))
                l.BookTitle = registro["BookTitle"].ToString();
            
            return l;
        }

        protected override void SetTabela() {
            Tabela = "Loan";
            NomeSpListagem = "spListagem_Loan";
        }
    }
}