﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        // GET: HelloWorld
        public ActionResult Index()
        {
            return View();
        }
        //GET: HelloWorld/Welcome
        public ActionResult Welcome(string name,int num=1)
        {
            ViewBag.Message ="Hello "+ name;
            ViewBag.Num = num;
            return View();
        }
        [HttpPost]
        public string Update()
        {
            var id = Request["myid"];
            var roll = Request["roll"];
            return "Updated data.";
        }
        [HttpPost]
        public string Update1(FormCollection values)
        {
            var id = values["myid"];
            var roll = values["roll"];
            return "Updated data.";
        }
    }
}