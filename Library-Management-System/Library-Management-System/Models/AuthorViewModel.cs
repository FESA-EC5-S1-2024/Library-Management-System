using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Models {
    public class AuthorViewModel : PadraoViewModel {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
