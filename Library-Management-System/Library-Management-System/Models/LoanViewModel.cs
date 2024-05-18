using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Models {
    public class LoanViewModel : PadraoViewModel {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // campos que não existem fisicamente na tabela de Loan
        public string UserName { get; set; }
        public string BookTitle { get; set; }
    }
}
