using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Data.SqlTypes;

namespace Library_Management_System.Controllers
{
    public class LoanController : PadraoController<LoanViewModel> {
        private readonly UserDAO UserDAO;
        private readonly BookDAO BookDAO;

        public LoanController()
        {
            DAO = new LoanDAO();
            UserDAO = new UserDAO();
            BookDAO = new BookDAO();
            ExigeAdmin = false;
        }
        protected override void ValidaDados(LoanViewModel model, string operacao) {
            base.ValidaDados(model, operacao);

            if (model.UserId <= 0)
                ModelState.AddModelError("UserId", "Adicione um Usuário.");

            if (model.BookId <= 0)
                ModelState.AddModelError("BookId", "Adicione um Livro.");

            if (model.LoanDate > DateTime.Now)
                ModelState.AddModelError("LoanDate", "Data de empréstimo inválida!");

            if (model.DueDate < model.LoanDate)
                ModelState.AddModelError("DueDate", "Data de vencimento inválida!");
        }

        protected override void PreencheDadosParaView(string Operacao, LoanViewModel model) {
            base.PreencheDadosParaView(Operacao, model);
            if (Operacao == "I") {
                model.LoanDate = DateTime.Now;
                model.DueDate = model.LoanDate.AddDays(7);
            }
            else if (Operacao == "A") {
                model.ReturnDate = DateTime.Now;
            }

            PreparaListaUsuariosParaCombo();
            PreparaListaLivrosParaCombo();
        }

        private void PreparaListaUsuariosParaCombo() {
            var usuarios = UserDAO.Listagem();
            List<SelectListItem> listaUsuarios = new List<SelectListItem>
            {
                new SelectListItem("Selecione um usuário...", "0")
            };
            foreach (var usuario in usuarios) {
                SelectListItem item = new SelectListItem(usuario.Name, usuario.Id.ToString());
                listaUsuarios.Add(item);
            }
            ViewBag.Usuarios = listaUsuarios;
        }

        private void PreparaListaLivrosParaCombo() {
            var livros = BookDAO.Listagem();
            List<SelectListItem> listaLivros = new List<SelectListItem>
            {
                new SelectListItem("Selecione um livros...", "0")
            };
            foreach (var livro in livros) {
                SelectListItem item = new SelectListItem(livro.Title, livro.Id.ToString());
                listaLivros.Add(item);
            }
            ViewBag.Livros = listaLivros;
        }

        public override IActionResult Index()
        {
            if (!HelperController.VerificaAdmin(HttpContext.Session))
            {
                try
                {
                    string userId = HttpContext.Session.GetString("UserId");
                    LoanDAO loandao = new LoanDAO();
                    var lista = loandao.ConsultaEmprestimos(userId);
                    return View(NomeViewIndex, lista);
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else
            {
                return base.Index();
            }
        }

        public IActionResult ExibeConsultaAvancada()
        {
            try
            {
                return View("ConsultaAvancada");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult ObtemDadosConsultaAvancada(string usuario, string descricao, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                LoanDAO loandao = new LoanDAO();
                if (string.IsNullOrEmpty(usuario))
                    usuario = "";
                if (string.IsNullOrEmpty(descricao))
                    descricao = "";
                if (dataInicial.Date == Convert.ToDateTime("01/01/0001"))
                    dataInicial = SqlDateTime.MinValue.Value;
                if (dataFinal.Date == Convert.ToDateTime("01/01/0001"))
                    dataFinal = SqlDateTime.MaxValue.Value;
                if (!HelperController.VerificaAdmin(HttpContext.Session))
                {
                    string userId = HttpContext.Session.GetString("UserId");
                    UserDAO userdao = new UserDAO();
                    var userData = userdao.Consulta(Convert.ToInt32(userId));
                    usuario = userData.Name;
                }
                var lista = loandao.ConsultaAvancada(usuario, descricao, dataInicial, dataFinal);
                return PartialView("pvGridLoans", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }
    }
}