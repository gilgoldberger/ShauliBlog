using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClassExe3.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}