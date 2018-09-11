using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassExe3.Models;
using ClassExe3.DAL;
using System.Data.Entity;

namespace ClassExe3.Controllers
{
    public class BlogController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();

        public ActionResult BlogView(string author = "", string website = "", string freeSearch = "", int numOfComments = -1)
        {
            //Get all posts from the database
            var posts = from p in db.Posts select p;

            //Filter posts according to user input
            if (!String.IsNullOrEmpty(author))
            {
                posts = posts.Where(p => p.Author.Contains(author));
            }
            if (!String.IsNullOrEmpty(website))
            {
                posts = posts.Where(p => p.Website.Contains(website));
            }
            if (!String.IsNullOrEmpty(freeSearch))
            {
                posts = posts.Where(p => p.Content.Contains(freeSearch));
            }
            if (numOfComments != -1)
            {
                posts = posts.Where(p => p.Comments.Count > numOfComments);
            }
            return View(posts);
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            //Find the Post related to the comment
            Post post = db.Posts.SingleOrDefault(p => p.PostID == comment.PostID);
            if (post == null)
            {
                return BlogView("", "", "", -1);
            }

            //Add the comment to the database
            post.Comments.Add(comment);
            db.SaveChanges();
            return Redirect("/");
        }
        public int PostsNumber(string name)
        {
            //Find the number of posts wrote by the comment author
            var posts = from comment in db.Comments join post in db.Posts on name equals post.Author select post.PostID;
            posts = posts.Distinct();
            return (posts.Count());
        }

        public ActionResult TopPostView()
        {
            //Find the post with the most comments
            var posts = from p in db.Comments
                        group p by p.PostID into g
                        select new { Key = g.Key, Count = g.Count() };
            var AnoPost = posts.OrderByDescending(p => p.Count).First();
            Post post = db.Posts.Find(AnoPost.Key);
            return View(post);
        }

        public ActionResult UpdateTopPostView()
        {
            //Find the post with the most comments
            var posts = from p in db.Comments
                        group p by p.PostID into g
                        select new { Key = g.Key, Count = g.Count() };
            var AnoPost = posts.OrderByDescending(p => p.Count).First();
            Post post = db.Posts.Find(AnoPost.Key);
            return PartialView("PartialTopPostView", post);
        }
    }
}
