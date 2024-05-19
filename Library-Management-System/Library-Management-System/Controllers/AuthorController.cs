using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;

namespace Library_Management_System.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorDAO _authorDAO;

        public AuthorController()
        {
            _authorDAO = new AuthorDAO();
        }

        public IActionResult Index()
        {
            var authors = _authorDAO.Listagem();
            return View(authors);
        }

        public IActionResult Details(int id)
        {
            var author = _authorDAO.Consulta(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _authorDAO.Insert(author);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            return View(author);
        }

        public IActionResult Edit(int id)
        {
            var author = _authorDAO.Consulta(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _authorDAO.Update(author);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            return View(author);
        }

        public IActionResult Delete(int id)
        {
            var author = _authorDAO.Consulta(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _authorDAO.Delete(id);
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