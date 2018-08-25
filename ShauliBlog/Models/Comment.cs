using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassExe3.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        [ForeignKey("Post")]
        public int PostID{ get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string Content { get; set; }
        [DisplayName("Related Post")]
        public Post Post { get; set; }
    }
}