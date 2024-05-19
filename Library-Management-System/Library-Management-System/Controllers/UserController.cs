using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDAO _userDAO;

        public UserController()
        {
            _userDAO = new UserDAO();
        }

        public IActionResult Index()
        {
            var users = _userDAO.Listagem();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _userDAO.Consulta(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                _userDAO.Insert(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var user = _userDAO.Consulta(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                _userDAO.Update(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Delete(int id)
        {
            var user = _userDAO.Consulta(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _userDAO.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}