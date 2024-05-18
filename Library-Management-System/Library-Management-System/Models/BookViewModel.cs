using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Models {
    public class BookViewModel : PadraoViewModel {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public byte[] Image { get; set; }

        // campos que não existem fisicamente na tabela de Book
        public string AuthorName { get; set; }
        public string CategoryDescription { get; set; }
    }
}
