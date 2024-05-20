using Library_Management_System.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDAO DAO;
        public LoginController()
        {
            DAO = new UserDAO();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FazLogin(string email, string senha)
        {
            var usuario = DAO.ConsultaUsuario(email);
            if (usuario != null && usuario.Password == senha)
            {
                HttpContext.Session.SetString("Logado", "true");
                HttpContext.Session.SetString("Admin", usuario.TypeId == 1 ? "true" : "false");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Erro = "Email ou senha inválidos!";
                return View("Index");
            }
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
