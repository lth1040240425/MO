using Demo.Models;
using Demo.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        public IUserService _userService { get; set; }
        public ActionResult Index()
        {
            var member = new Member();
            member.Address = "1";
            member.Birthday = DateTime.Now;
            member.CityId = Guid.NewGuid();
            member.Gender = 2;
            member.Id = Guid.NewGuid();
            member.Name = "3";
            member.NickName = "4";
            member.PhoneNum = "5";
            member.ProvinceId = Guid.NewGuid();
            _userService.Register(member);
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
