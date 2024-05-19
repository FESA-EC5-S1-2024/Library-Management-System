using Microsoft.AspNetCore.Http;
using System;

namespace Library_Management_System.Models
{
    public class BookViewModel : PadraoViewModel {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }

        /// <summary>
        /// Imagem recebida do form pelo controller
        /// </summary>
        public IFormFile Image { get; set; }


        // campos que não existem fisicamente na tabela de Book
        public string AuthorName { get; set; }
        public string CategoryDescription { get; set; }

        /// <summary>
        /// Imagem em bytes pronta para ser salva
        /// </summary>
        public byte[] ImageByte { get; set; }

        /// <summary>
        /// Imagem usada para ser enviada ao form no formato para ser exibida
        /// </summary>
        public string ImageBase64
        {
            get
            {
                if (ImageByte != null)
                    return Convert.ToBase64String(ImageByte);
                else
                    return string.Empty;
            }
        }

    }
}
