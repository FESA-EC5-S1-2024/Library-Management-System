using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;

namespace Library_Management_System.Controllers
{
    public class CategoryController : PadraoController<CategoryViewModel>
    {
        public CategoryController()
        {
            DAO = new CategoryDAO();
        }

        protected override void ValidaDados(CategoryViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Description))
                ModelState.AddModelError("Description", "Preencha a descrição.");
        }

        protected override void PreencheDadosParaView(string Operacao, CategoryViewModel model) { }
    }
}