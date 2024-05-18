using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;

namespace Library_Management_System.Controllers
{
    public class BookController : Controller
    {
        private readonly BookDAO _bookDAO;
        private readonly AuthorDAO _authorDAO;
        private readonly CategoryDAO _categoryDAO;

        public BookController()
        {
            _bookDAO = new BookDAO();
            _authorDAO = new AuthorDAO();
            _categoryDAO = new CategoryDAO();
        }

        public IActionResult Index()
        {
            var books = _bookDAO.Listagem();
            return View(books);
        }

        public IActionResult Details(int id)
        {
            var book = _bookDAO.Consulta(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult Create()
        {
            ViewBag.Authors = _authorDAO.Listagem();
            ViewBag.Categories = _categoryDAO.Listagem();
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _bookDAO.Insert(book);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            ViewBag.Authors = _authorDAO.Listagem(); // Recarregar em caso de erro
            ViewBag.Categories = _categoryDAO.Listagem(); // Recarregar em caso de erro
            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var book = _bookDAO.Consulta(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Authors = _authorDAO.Listagem();
            ViewBag.Categories = _categoryDAO.Listagem();
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _bookDAO.Update(book);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            ViewBag.Authors = _authorDAO.Listagem(); // Recarregar em caso de erro
            ViewBag.Categories = _categoryDAO.Listagem(); // Recarregar em caso de erro
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            var book = _bookDAO.Consulta(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _bookDAO.Delete(id);
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