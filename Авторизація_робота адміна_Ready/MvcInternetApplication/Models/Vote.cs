using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcInternetApplication.Models
{
    public class Vote
    {
        public int FilmID { get; set; }
        public int Vote_Plus { get; set; }
        public int Vote_Minus { get; set; }
    }
}