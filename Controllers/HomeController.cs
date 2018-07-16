
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wall.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Wall.Controllers
{
    public class HomeController : Controller
    {

        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
            _context.SaveChanges();

        }


        [HttpGet("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet("Registerd")]
        public IActionResult SignIn()
        {
            Console.WriteLine("Got inside registerd");


            return View("SignIn");
        }





        [HttpPost("Home/crate")]

        public IActionResult Create(User user)
        {
            List<User> Users = _context.users.Include(usr => usr.Msgs).ToList();
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            user.Confirm_Password = Hasher.HashPassword(user, user.Confirm_Password);

            user.Created_At = DateTime.Now;
            user.Updated_At = DateTime.Now;

            _context.Add(user);
            _context.SaveChanges();
            ViewBag.user = user;

            HttpContext.Session.SetString("Email", user.Email);


            return RedirectToAction("DashboardView");
        }

        [HttpPost("Home/login")]
        public IActionResult Login(string Email, string Password)
        {
            List<User> Users = _context.users.Include(usr => usr.Msgs).ToList();

            User LogedUser = _context.users.SingleOrDefault(usr => usr.Email == Email);




            if (LogedUser != null)
            {
                ViewBag.user = LogedUser;
                HttpContext.Session.SetString("Email", LogedUser.Email);

                return RedirectToAction("DashboardView");

            }
            else
            {
                Console.WriteLine("Invalid User");
                ViewBag.err = "Invalid User";
                return View("SignIn");
            }

        }


        public IActionResult DashboardView()
        {
            List<User> Users = _context.users.Include(usr => usr.Msgs).ToList();



            List<messages> Messages = _context.messages.Include(m=>m.comments).ToList();

            string Email = HttpContext.Session.GetString("Email");

            User user = _context.users.SingleOrDefault(usr => usr.Email == Email);


            


            List<User>usrs = _context.users.ToList(); 






            ViewBag.allUsers = usrs;

            ViewBag.user = user;
            return View("Dashboard");


        }


        [HttpPost("Home/addMsg")]
        public IActionResult addMsg(string newMsg)
        {


            // Console.WriteLine("In adding message trying to add " + newMsg+"for user "+LogedUser.First_Name);

            messages msg = new messages();
            msg.Messages_Text = newMsg;
            msg.Created_At = DateTime.Now;
            msg.Updated_At = DateTime.Now;


            string Email = HttpContext.Session.GetString("Email");

            User LogedUser = _context.users.SingleOrDefault(usr => usr.Email == Email);


            LogedUser.Msgs.Add(msg);
            _context.SaveChanges();

            msg.UserId = LogedUser.UserId;

            return RedirectToAction("DashboardView");
        }


        
        [HttpPost("Home/addComnt")]
        public IActionResult addComnt(string newComnt , int msgId){
            
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Message Id: "+msgId);

            string Email = HttpContext.Session.GetString("Email");

            User LogedUser = _context.users.SingleOrDefault(usr => usr.Email == Email);
            
            messages msg = _context.messages.SingleOrDefault(m=>m.MessagesId==msgId);

            comments comnt = new comments();
            comnt.MessagesId=msg.MessagesId;
            comnt.Comment_Text=newComnt;
            comnt.UserName=LogedUser.First_Name+" "+LogedUser.Last_Name;
            comnt.Created_At=DateTime.Now;
            comnt.Updated_At=DateTime.Now;

            
            msg.comments.Add(comnt);
            _context.SaveChanges();

            return RedirectToAction("DashboardView");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
