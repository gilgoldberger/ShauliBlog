using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ClassExe3.Models
{
    public class CommentCityView
    {
        [DisplayName("Number of comments")]
        public int numOfComments { get; set; }
        public string City { get; set; }
    }
}