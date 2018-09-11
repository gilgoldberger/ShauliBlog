using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClassExe3.Models;
using ClassExe3.DAL;
using Facebook;
using System.Dynamic;
using System.IO;

namespace ClassExe3.Controllers
{
    public class FanClubController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();

        public ActionResult Index(string firstName, string lastName, Gender? gender)
        {
            // Set the values to the drop-down list
            var GenderList = new List<Gender>();
            var GenderQuery = from f in db.Fans orderby f.gender select f.gender;
            GenderList.AddRange(GenderQuery.Distinct());
            ViewBag.gender = new SelectList(GenderList);

            // Search according to user's input
            var fans = from f in db.Fans select f;
            if (!String.IsNullOrEmpty(firstName))
            {
                fans = fans.Where(f => f.firstName.Contains(firstName));
            }
            if (!String.IsNullOrEmpty(lastName))
            {
                fans = fans.Where(f => f.lastName.Contains(lastName));
            }
            if (gender != null)
            {
                fans = fans.Where(f => f.gender == gender);
            }

            //Return the results of the filters
            return View(fans);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the fan.
            Fan fan = db.Fans.Find(id);

            //Check if the fan exist and return it
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,firstName,lastName,gender,birthDate,City,Latitude,Longitude,seniority")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                //Add Fan to the database
                db.Fans.Add(fan);
                db.SaveChanges();
               
                //Crate an object with all parameters for the post
                dynamic messagePost = new ExpandoObject();
                messagePost.message = "Hi all, Lets congratulate " + fan.firstName + " for joining our awesome fan club!";

                //Open a session to the facebook page
                FacebookClient app = new FacebookClient("EAAJO0xAYtwQBAIBjC9GtpUiKZBMNeakebrfsW8eGLDTr3HBpSVywia0CNJ4T9xq1hNmFAmujlkWu8YcT2QrseLbwkfd5kTq3O31hnADQpKppESFY7ioe0l9QZBVUG6ai1cfR3vpFAFFgmdirBTx0rAA1pjBC6PYgeeqbPafAZDZD");
               
                //Try to post the information to the facebook page
                try
                {
                    var result = app.Post("/355852721418666/feed", messagePost);
                }

                //Log the Exception if an error occured
                catch (FacebookOAuthException ex)
                {
                    string filePath = @"C:\Logs\FacebookLog.txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Error Posting at Facebook about new fan.");
                        writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                        writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(fan);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the Fan in the database
            Fan fan = db.Fans.Find(id);

            //If the fan exist return it to the view
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,firstName,lastName,gender,birthDate,City,Latitude,Longitude,seniority")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                //Update the fan according to the input from the user
                db.Entry(fan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fan);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the fan in the database
            Fan fan = db.Fans.Find(id);
            //If the fan exist return it to the view
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Delete the fan from the database
            Fan fan = db.Fans.Find(id);
            db.Fans.Remove(fan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //close connection to the database
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
