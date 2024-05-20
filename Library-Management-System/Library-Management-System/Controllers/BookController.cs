using Library_Management_System.DAO;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Library_Management_System.Controllers
{
    public class BookController : PadraoController<BookViewModel>
    {
        private readonly AuthorDAO AuthorDAO;
        private readonly CategoryDAO CategoryDAO;

        public BookController()
        {
            DAO = new BookDAO();
            AuthorDAO = new AuthorDAO();
            CategoryDAO = new CategoryDAO();
            ExigeAdmin = false;
        }

        protected override void ValidaDados(BookViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Title))
                ModelState.AddModelError("Title", "Preencha o título.");
            if (string.IsNullOrEmpty(model.ISBN))
                ModelState.AddModelError("ISBN", "Preencha o ISBN.");
            if (model.AuthorId <= 0)
                ModelState.AddModelError("AuthorId", "Informe o código do autor.");
            if (model.CategoryId <= 0)
                ModelState.AddModelError("CategoryId", "Informe o código da categoria.");
            if (model.PublishedYear > DateTime.Now.Year)
                ModelState.AddModelError("PublishedYear", "Ano de publicação inválido!");

            // Imagem obrigatória no Insert
            if (model.Image == null && operacao == "I")
                ModelState.AddModelError("Image", "Escolha uma imagem.");
            if (model.Image != null && model.Image.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Image", "Imagem limitada a 2 mb.");
            if (ModelState.IsValid)
            {
                // No Update, buscar imagem salva caso não seja informada
                if (operacao == "A" && model.Image == null)
                {
                    BookViewModel book = DAO.Consulta(model.Id);
                    model.ImageByte = book.ImageByte;
                }
                else
                {
                    model.ImageByte = ConvertImageToByte(model.Image);
                }
            }
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        protected override void PreencheDadosParaView(string Operacao, BookViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            if (Operacao == "I")
                model.PublishedYear = DateTime.Now.Year;

            PreparaListaAutoresParaCombo();
            PreparaListaCategoriasParaCombo();
        }

        private void PreparaListaAutoresParaCombo()
        {
            var autores = AuthorDAO.Listagem();
            List<SelectListItem> listaAutores = new List<SelectListItem>
            {
                new SelectListItem("Selecione um autor...", "0")
            };
            foreach (var autor in autores)
            {
                SelectListItem item = new SelectListItem(autor.Name, autor.Id.ToString());
                listaAutores.Add(item);
            }
            ViewBag.Autores = listaAutores;
        }

        private void PreparaListaCategoriasParaCombo()
        {
            var categorias = CategoryDAO.Listagem();
            List<SelectListItem> listaCategorias = new List<SelectListItem>
            {
                new SelectListItem("Selecione uma categoria...", "0")
            };
            foreach (var categoria in categorias)
            {
                SelectListItem item = new SelectListItem(categoria.Description, categoria.Id.ToString());
                listaCategorias.Add(item);
            }
            ViewBag.Categorias = listaCategorias;
        }
    }
}