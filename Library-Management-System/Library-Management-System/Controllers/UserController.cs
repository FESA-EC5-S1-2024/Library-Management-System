using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;

namespace Library_Management_System.Controllers
{
    public class UserController : PadraoController<UserViewModel> {
        public UserController()
        {
            DAO = new UserDAO();
            NovoUsuario = true;
        }

        protected override void ValidaDados(UserViewModel model, string operacao) {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Name))
                ModelState.AddModelError("Name", "Preencha o nome.");

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Preencha o Email.");
            else if (!Regex.IsMatch(model.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                ModelState.AddModelError("Email", "Por favor, insira um endereço de e-mail válido.");
            }

            if (string.IsNullOrEmpty(model.Password))
                ModelState.AddModelError("Password", "Adicione uma senha.");
        }

        protected override void PreencheDadosParaView(string Operacao, UserViewModel model) { 
            if(Operacao == "I") {
                model.RegistrationDate = DateTime.Now;
            }
        }
    }
}