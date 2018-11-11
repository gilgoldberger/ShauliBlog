using ClassExe3.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassExe3.Models;
using System.Data;

namespace ClassExe3.Controllers
{
    public class AboutController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();
        public ActionResult Index()
        {
            int i = 0;

            // Select all the comments - group by cities
            IEnumerable<CommentCityView> query = db.Comments.GroupBy(c => c.City).Select(g => new CommentCityView{ City = g.Key, numOfComments = g.Count() });

            // Select the coordinates of the fans from the DB
            ViewBag.location = db.Fans.Select(location => new LocationView { Latitude = location.Latitude, Longitude = location.Longitude });
            var arr = ViewBag.location as IEnumerable<object>;
            double[] latitude = new double[arr.Count()];
            double[] longtitude = new double[arr.Count()];
            foreach (LocationView lv in ViewBag.location)
            {
                latitude[i] = lv.Latitude;
                longtitude[i++] = lv.Longitude;
            }
            ViewBag.latitude = latitude;
            ViewBag.longtitude = longtitude;
            return View(query);
        }
    }
}