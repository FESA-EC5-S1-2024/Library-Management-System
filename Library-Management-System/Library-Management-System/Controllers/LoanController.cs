using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    }
}