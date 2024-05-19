using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;

namespace Library_Management_System.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryDAO _categoryDAO;

        public CategoryController()
        {
            _categoryDAO = new CategoryDAO();
        }

        public IActionResult Index()
        {
            var categories = _categoryDAO.Listagem();
            return View(categories);
        }

        public IActionResult Details(int id)
        {
            var category = _categoryDAO.Consulta(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _categoryDAO.Insert(category);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryDAO.Consulta(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _categoryDAO.Update(category);
                        transaction.Complete();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        return View("Error", new ErrorViewModel(e.Message));
                    }
                }
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryDAO.Consulta(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _categoryDAO.Delete(id);
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