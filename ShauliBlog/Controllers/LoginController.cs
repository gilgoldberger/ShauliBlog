using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClassExe3.Models;
using ClassExe3.DAL;
using System.Web.Security;

namespace ClassExe3.Controllers
{
    public class LoginController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();
        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult Authenticate(Account account)
        {
            if(ModelState.IsValid)
            {
                // Check if user is valid and transfer fo homepage
                List<Account> accountValidate = (from a in db.Accounts
                                                 where (a.AccountID == account.AccountID) && (a.Password == account.Password)
                                                 select a).ToList<Account>();
                if(accountValidate.Count == 1)
                {
                    // Authantication succeeded
                    FormsAuthentication.SetAuthCookie("cookie", true);
                    ViewBag.newUser = User.Identity;
                    return RedirectToRoute("DefaultManagement");
                }
                else
                {
                    return View("Login", account);
                }
            }
            else
            {
                return View("Login", account);
            }
        }
    }
}