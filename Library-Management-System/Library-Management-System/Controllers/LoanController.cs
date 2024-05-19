using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;

namespace Library_Management_System.Controllers
{
    public class LoanController : Controller
    {
        private readonly LoanDAO _loanDAO;
        private readonly UserDAO _userDAO;
        private readonly BookDAO _bookDAO;

        public LoanController()
        {
            _loanDAO = new LoanDAO();
            _userDAO = new UserDAO();
            _bookDAO = new BookDAO();
        }

        public IActionResult Index()
        {
            var loans = _loanDAO.Listagem();
            return View(loans);
        }

        public IActionResult Details(int id)
        {
            var loan = _loanDAO.Consulta(id);
            if (loan == null)
            {
                return NotFound();
            }
            return View(loan);
        }

        public IActionResult Create()
        {
            ViewBag.Users = _userDAO.Listagem();
            ViewBag.Books = _bookDAO.Listagem();
            return View();
        }

        [HttpPost]
        public IActionResult Create(LoanViewModel loan)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _loanDAO.Insert(loan);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            ViewBag.Users = _userDAO.Listagem(); // Recarregar em caso de erro
            ViewBag.Books = _bookDAO.Listagem(); // Recarregar em caso de erro
            return View(loan);
        }

        public IActionResult Edit(int id)
        {
            var loan = _loanDAO.Consulta(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewBag.Users = _userDAO.Listagem();
            ViewBag.Books = _bookDAO.Listagem();
            return View(loan);
        }

        [HttpPost]
        public IActionResult Edit(LoanViewModel loan)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _loanDAO.Update(loan);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            ViewBag.Users = _userDAO.Listagem(); // Recarregar em caso de erro
            ViewBag.Books = _bookDAO.Listagem(); // Recarregar em caso de erro
            return View(loan);
        }

        public IActionResult Delete(int id)
        {
            var loan = _loanDAO.Consulta(id);
            if (loan == null)
            {
                return NotFound();
            }
            return View(loan);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _loanDAO.Delete(id);
                    transaction.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return View("Error", new ErrorViewModel(e.Message));
                }
            }
        }
    }
}