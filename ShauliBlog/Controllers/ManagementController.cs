using ClassExe3.DAL;
using ClassExe3.Models;
using Facebook;
using System;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ClassExe3.Controllers
{
    [Authorize]
    public class ManagementController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();

        public ActionResult Index(string postTitle, string author, string commentAuthor, string commentContent)
        {
            //Add all posts to the ViewBag
            ViewBag.Posts = from p in db.Posts orderby p.Title select p;
           
            // Search according to user's input
            var posts = from p in db.Posts select p;
            var comments = from c in db.Comments select c;
            if (!string.IsNullOrEmpty(postTitle))
            {
                int postID = Convert.ToInt32(postTitle);
                posts = posts.Where(p => p.PostID.Equals(postID));
            }
            if (!String.IsNullOrEmpty(author))
            {
                posts = posts.Where(p => p.Author.Contains(author));
            }
            if(!String.IsNullOrEmpty(commentAuthor))
            {
                comments = comments.Where(p => p.Author.Contains(commentAuthor));
            }
            if(!String.IsNullOrEmpty(commentContent))
            {
                comments = comments.Where(p => p.Content.Contains(commentContent));
            }
            //Use left outer join to show posts.
            var view = from p in posts
                       join c in comments on p.PostID equals c.PostID into g
                       from post in g.DefaultIfEmpty()
                       select p;
            //return the posts.
            return View(view.Distinct());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the post in the database.
            Post post = db.Posts.Find(id);
            //return it if it's exist.
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        public ActionResult Create()
        {
            //Return the Create view.
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Author,Title,PublishDate,Content,Website,City,Image,Video")] Post post)
        {
            if (ModelState.IsValid)
            {
                //Add the post to the database
                post.PublishDate = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                //Creating an object for the facebook page.
                FacebookClient app = new FacebookClient("EAAJO0xAYtwQBAIBjC9GtpUiKZBMNeakebrfsW8eGLDTr3HBpSVywia0CNJ4T9xq1hNmFAmujlkWu8YcT2QrseLbwkfd5kTq3O31hnADQpKppESFY7ioe0l9QZBVUG6ai1cfR3vpFAFFgmdirBTx0rAA1pjBC6PYgeeqbPafAZDZD");
                //Creating object with parameters of the post.
                dynamic messagePost = new ExpandoObject();
                messagePost.message = "Hi guys, Check out new post wrote by "; //" + post.Author + "!";
                //if image is available - post it too.
                if (post.Image != null)
                {
                    messagePost.url = post.Image;
                    //Try to post the information on the facebook page
                    try
                    {
                        var result = app.Post("/355852721418666/photos", messagePost);

                    }
                    //Log the Exception if an error occured. 
                    catch (FacebookOAuthException ex)
                    {
                        string filePath = @"C:\Logs\FacebookLog.txt";
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine("Error Posting at Facebook about a new post with photo.");
                            writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                               "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                            writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                        }
                    }
                }
                else
                {
                    //Try to post the information to facebook without the image
                    try
                    {
                        var result = app.Post("/355852721418666/feed", messagePost);
                    }
                    //Log the Exception if an error occured.
                    catch (FacebookOAuthException ex)
                    {
                        string filePath = @"C:\Logs\FacebookLog.txt";
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine("Error Posting at Facebook about a new post.");
                            writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                               "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                            writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find the Post in the database.
            Post post = db.Posts.Find(id);
            //If the post exist return it
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,Author,Title,PublishDate,Content,Website,City,Image,Video")] Post post)
        {
            if (ModelState.IsValid)
            {
                //Edit the post details and save the changes
                db.Entry(post).State = EntityState.Modified;
                post.PublishDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the post in the database.
            Post post = db.Posts.Find(id);
            //If the post exist return it to the view
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Delete the post from the database.
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Manage(int id)
        {
            //Return all comments of the given post
            Post post = db.Posts.SingleOrDefault(p => p.PostID == id);

            return View(post.Comments.ToList());
        }
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the comment in the database
            Comment comment = db.Comments.Find(id);
            //If the comment exist return it to the view
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        [HttpPost, ActionName("DeleteComment")]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            //Delete the comment from the database
            Comment comment = db.Comments.Find(id);
            if (comment != null)
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the comment in the database
            Comment comment = db.Comments.Find(id);
            //If the comment exist return it to the view
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment([Bind(Include = "CommentID,PostID,Title,Author,Website,City,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                //Update the comment with new information in the database
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }
    }
}
