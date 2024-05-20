using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Library_Management_System.Controllers
{
    public class PadraoController<T> : Controller where T : PadraoViewModel
    {
        protected bool ExigeAutenticacao { get; set; } = true;
        protected bool ExigeAdmin { get; set; } = true;
        protected bool NovoUsuario { get; set; } = false;
        protected PadraoDAO<T> DAO { get; set; }
        protected string NomeViewIndex { get; set; } = "index";
        protected string NomeViewForm { get; set; } = "form";
        protected virtual void ValidaDados(T model, string operacao) { }
        protected virtual void PreencheDadosParaView(string Operacao, T model) { }

        public virtual IActionResult Index() {
            if (HelperController.VerificaUserLogado(HttpContext.Session))
            {
                try
                {
                    var lista = DAO.Listagem();
                    return View(NomeViewIndex, lista);
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else
            {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public virtual IActionResult Create() {
            if (HelperController.VerificaAdmin(HttpContext.Session) || NovoUsuario) {
                try {
                    ViewBag.Operacao = "I";
                    T model = Activator.CreateInstance<T>();
                    PreencheDadosParaView("I", model);
                    return View(NomeViewForm, model);
                }
                catch (Exception erro) {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public virtual IActionResult Save(T model, string Operacao)
        {
            if (HelperController.VerificaAdmin(HttpContext.Session) || NovoUsuario) {
                try {
                    ValidaDados(model, Operacao);
                    if (ModelState.IsValid == false) {
                        ViewBag.Operacao = Operacao;
                        PreencheDadosParaView(Operacao, model);
                        return View(NomeViewForm, model);
                    }
                    else {
                        if (Operacao == "I")
                            DAO.Insert(model);
                        else
                            DAO.Update(model);

                        if (NovoUsuario) {
                            return RedirectToAction("Index", "Login");
                        }
                        else {
                            return RedirectToAction(NomeViewIndex);
                        }
                    }
                }
                catch (Exception erro) {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public IActionResult Edit(int id)
        {
            if (HelperController.VerificaAdmin(HttpContext.Session)) {
                try {
                    ViewBag.Operacao = "A";
                    var model = DAO.Consulta(id);
                    if (model == null)
                        return RedirectToAction(NomeViewIndex);
                    else {
                        PreencheDadosParaView("A", model);
                        return View(NomeViewForm, model);
                    }
                }
                catch (Exception erro) {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public IActionResult Delete(int id)
        {
            if (HelperController.VerificaAdmin(HttpContext.Session)) {
                try {
                    DAO.Delete(id);
                    return RedirectToAction(NomeViewIndex);
                }
                catch (Exception erro) {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ExigeAutenticacao && !HelperController.VerificaUserLogado(HttpContext.Session)) {
                if (NovoUsuario) {
                    base.OnActionExecuting(context);
                }
                else {
                    context.Result = RedirectToAction("Index", "Login");
                }
                
            }
            else if (ExigeAdmin && !HelperController.VerificaAdmin(HttpContext.Session)) {
                context.Result = new RedirectToActionResult("AcessoNegado", "Error", null);
            }
            else {
                base.OnActionExecuting(context);
            }
        }
    }
}
