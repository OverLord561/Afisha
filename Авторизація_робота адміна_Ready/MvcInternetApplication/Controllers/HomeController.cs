using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcInternetApplication.Filters;
using MvcInternetApplication.Models;
using WebMatrix.WebData;

namespace MvcInternetApplication.Controllers
{
    [InitializeSimpleMembershipAttribute]
    public class HomeController : Controller
    {
        
        private AFISHAContext db = new AFISHAContext();
        
        public ActionResult Index()
        {

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
         
            
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize] // Запрещены анонимные обращения к данной странице
        public ActionResult Cabinet()
        {
            ViewBag.FileName = TempData.Peek("FileName") as string;
            ViewBag.UserMail = TempData.Peek("UserMail") as string;

            ViewBag.UserNAME = TempData.Peek("_UserName") as string;
            ViewBag.Message = "Private Page.";

            return View();
        }


        [HttpPost]
        [Authorize]
        public ActionResult Cabinet(HttpPostedFileBase file)
        {
            string fileName = Guid.NewGuid().ToString();
            TempData["FileName"] = "/Content/Images/Uploads/" + fileName + ".jpg";
            ViewBag.FileName = TempData.Peek("FileName") as string;
            string extension = Path.GetExtension(file.FileName);
            fileName += extension;

            List<string> extensions = new List<string>() { ".txt", ".png", ".jpg", ".pdf", ".zip" };
            if (extensions.Contains(extension))
            {
                file.SaveAs(Server.MapPath("/Content/Images/Uploads/" + fileName));
                ViewBag.Message = "Файл сохранен";
            }
            else
            {
                ViewBag.Message = "Ошибка. Допустимые расширения файлов - '.txt', '.png', '.jpg', '.pdf', '.zip'";
            }

            return RedirectToAction("Cabinet");
        }






        [Authorize(Roles = "Admin")] // К данному методу действия могут получать доступ только пользователи с ролью Admin
        public ActionResult AdminPanel()
        {
            ViewBag.Message = "Admin Panel.";

            return View();
        }

        [Authorize(Roles = "Admin, Moderator")] // К данному методу действия могут получать доступ только пользователи с ролью Admin и Moderator
        public ActionResult ModeratorPanel()
        {
            ViewBag.Message = "Moderator Panel.";

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanel(string list1)
        {
            var Films = db.Films.ToList();
            var Sessions=db.Sessions.ToList();;
            var MovieHouses=db.MovieHouses.ToList();;
            var UserComments = db.UserComments.ToList();
           
            switch (list1)
            {
                case "Films": return RedirectToAction("Index", "Films");
                case "Sessions": return RedirectToAction("Index", "Sessions");
                case "MovieHouses": return RedirectToAction("Index", "MovieHouses");
                case "UserComments": return RedirectToAction("Index", "UserComments");
                default: return View("Index");
            }
            
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser(string NameOfUser, string ListOfUser, string PasOfUser)
        {
            SimpleRoleProvider roles = (SimpleRoleProvider)Roles.Provider;
            SimpleMembershipProvider membership = (SimpleMembershipProvider)Membership.Provider;

             

            if (membership.GetUser(NameOfUser, false) != null)
            {
                return Content("Такий користувач уже існує");
            }

            else
            {
                membership.CreateUserAndAccount(NameOfUser, PasOfUser); // создание пользователя
                roles.AddUsersToRoles(new[] { NameOfUser }, new[] { ListOfUser }); // установка роли для пользователя
            }

            return RedirectToAction("Index", "Home");
        }

       

    }
}

