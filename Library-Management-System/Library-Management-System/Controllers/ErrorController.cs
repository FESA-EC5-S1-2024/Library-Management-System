using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Controllers {
    public class ErrorController : Controller {
        public IActionResult AcessoNegado() {
            return View("Error", new ErrorViewModel("Acesso Negado"));
        }
    }
}
