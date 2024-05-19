using Library_Management_System.DAO;
using Library_Management_System.Models;
using System;

namespace Library_Management_System.Controllers
{
    public class AuthorController : PadraoController<AuthorViewModel>
    {

        public AuthorController()
        {
            DAO = new AuthorDAO();
        }

        protected override void ValidaDados(AuthorViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Name))
                ModelState.AddModelError("Nome", "Preencha o nome.");
            if (string.IsNullOrEmpty(model.Country))
                ModelState.AddModelError("CidadeId", "Informe o país.");
            if (model.Birthdate > DateTime.Now)
                ModelState.AddModelError("DataNascimento", "Data inválida!");
        }

        protected override void PreencheDadosParaView(string Operacao, AuthorViewModel model) { }
    }
}