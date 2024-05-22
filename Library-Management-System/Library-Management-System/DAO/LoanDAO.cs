using Library_Management_System.Controllers;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DAO {
    class LoanDAO : PadraoDAO<LoanViewModel> {
        protected override SqlParameter[] CriaParametros(LoanViewModel loan, bool isInsert = false) {
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!isInsert) {
                parametros.Add(new SqlParameter("LoanId", loan.Id));
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

        public List<LoanViewModel> ConsultaEmprestimos(string userId)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("UserId", Convert.ToInt32(userId))
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_Loan", p);
            List<LoanViewModel> lista = new List<LoanViewModel>();
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }

        public List<LoanViewModel> ConsultaAvancada(string usuario, string descricao, DateTime dataInicial, DateTime dataFinal)
        {
            SqlParameter[] p = {
             new SqlParameter("usuario", usuario),
             new SqlParameter("descricao", descricao),
             new SqlParameter("dataInicial", dataInicial),
             new SqlParameter("dataFinal", dataFinal),
             };
            var tabela = HelperDAO.ExecutaProcSelect("spConsultaAvancada_Loan", p);
            var lista = new List<LoanViewModel>();
            foreach (DataRow dr in tabela.Rows)
                lista.Add(MontaModel(dr));
            return lista;
        }
    }
}