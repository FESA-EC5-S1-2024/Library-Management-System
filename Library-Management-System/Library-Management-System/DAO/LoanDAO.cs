using Library_Management_System.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class LoanDAO : PadraoDAO<LoanViewModel> {
        protected override SqlParameter[] CriaParametros(LoanViewModel loan) {
            SqlParameter[] p = new SqlParameter[6];
            p[0] = new SqlParameter("LoanId", loan.Id);
            p[1] = new SqlParameter("UserId", loan.UserId);
            p[2] = new SqlParameter("BookId", loan.BookId);
            p[3] = new SqlParameter("LoanDate", loan.LoanDate);
            p[4] = new SqlParameter("DueDate", loan.DueDate);
            p[5] = new SqlParameter("ReturnDate", HelperDAO.NullAsDbNull(loan.ReturnDate));
            return p;
        }

        protected override LoanViewModel MontaModel(DataRow registro) {
            LoanViewModel l = new LoanViewModel();
            l.Id = Convert.ToInt32(registro["LoanId"]);
            l.UserId = Convert.ToInt32(registro["UserId"]);
            l.BookId = Convert.ToInt32(registro["BookId"]);
            l.LoanDate = Convert.ToDateTime(registro["LoanDate"]);
            l.DueDate = Convert.ToDateTime(registro["DueDate"]);
            l.ReturnDate = registro["ReturnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(registro["ReturnDate"]);
            return l;
        }

        protected override void SetTabela() {
            Tabela = "Loan";
            NomeSpListagem = "spListagemLoan";
        }
    }
}