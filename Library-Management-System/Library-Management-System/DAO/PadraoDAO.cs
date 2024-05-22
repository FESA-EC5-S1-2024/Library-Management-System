using Library_Management_System.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Linq;

namespace Library_Management_System.DAO {
    public abstract class PadraoDAO<T> where T : PadraoViewModel {
        public PadraoDAO() {
            SetTabela();
        }

        protected string Tabela { get; set; }
        protected string NomeSpListagem { get; set; } = "spListagem";
        protected string NomeSpDelete { get; set; } = "spDelete";
        protected abstract SqlParameter[] CriaParametros(T model, bool isInsert = false);
        protected abstract T MontaModel(DataRow registro);
        protected abstract void SetTabela();

        public virtual void Insert(T model) {
            HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(model, true));
        }

        public virtual void Update(T model) {
            HelperDAO.ExecutaProc("spUpdate_" + Tabela, CriaParametros(model, false));
        }

        public virtual void Delete(int id) {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id)
            };
            if (NomeSpDelete == "spDelete")
                p.Append(new SqlParameter("tabela", Tabela));
            HelperDAO.ExecutaProc(NomeSpDelete, p);
        }

        public virtual T Consulta(int id) {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public virtual List<T> Listagem() {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela),
                new SqlParameter("Ordem", "1") // 1 é o primeiro campo da tabela
            };
            var tabela = HelperDAO.ExecutaProcSelect(NomeSpListagem, p);
            List<T> lista = new List<T>();
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }
    }
}
