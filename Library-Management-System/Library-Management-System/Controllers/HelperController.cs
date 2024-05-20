using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Library_Management_System.Controllers
{
    public class HelperController : Controller
    {
        public static Boolean VerificaUserLogado(ISession session)
        {
            string logado = session.GetString("Logado");
            if (logado == null) {
                return false;
            }
            else {
                return true;
            }
        }
        public static Boolean VerificaAdmin(ISession session) {
            string admin = session.GetString("Admin");
            if (admin == "false") {
                return false;
            }
            else if (admin == null) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
