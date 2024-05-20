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

            if (string.IsNullOrEmpty(model.Name)) {
                ModelState.AddModelError("Name", "Preencha o nome.");
            }

            if (string.IsNullOrEmpty(model.Country)) {
                ModelState.AddModelError("Country", "Informe o país.");
            }

            if (model.Birthdate > DateTime.Now) {
                ModelState.AddModelError("DataNascimento", "Data inválida!");
            }
            else if (model.Birthdate < new DateTime(1753, 1, 1)) {
                ModelState.AddModelError("Birthdate", "Data inválida! A data de nascimento deve ser maior que 01/01/1753.");
            }
        }

        protected override void PreencheDadosParaView(string Operacao, AuthorViewModel model) { }
    }
}