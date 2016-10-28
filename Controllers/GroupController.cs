using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beltReviewer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using beltReviewer.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace beltReviewer.Controllers
{
    
    public class GroupController : Controller
    {

    private readonly GroupFactory groupFactory;
    private readonly UserFactory userFactory;

     public GroupController(GroupFactory group, UserFactory user) {
            userFactory = user;
            groupFactory = group;
        }
        // GET: /Home/
        [HttpGet]
        [Route("groups")]
        public IActionResult Group()
        {
            HttpContext.Session.SetString("first_name", "josh");
            HttpContext.Session.SetInt32("user_id", 15);
            var first_name = HttpContext.Session.GetString("first_name");
            ViewBag.first_name =  first_name;
            ViewBag.user_id =  HttpContext.Session.GetInt32("user_id");
           ViewBag.all_groups = groupFactory.FindAll();
            return View();
        }

        [HttpGet]
        [Route("groups/{id}")]
        public IActionResult Show(int id)
        {
           
           ViewBag.group =  groupFactory.FindById(id);
           ViewBag.members = groupFactory.FindUserGroups(id);
           
            return View();
        }

        [HttpPost]
        [Route("delete/{group_id}")]
        public IActionResult Delete(int group_id)
        {
            var user_id = HttpContext.Session.GetInt32("user_id");
            groupFactory.Delete(group_id);
           
            return RedirectToAction("Group");
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Route("groups")]
        public IActionResult Add(string name, string description)
        {
            var user_id = HttpContext.Session.GetInt32("user_id");
            groupFactory.Add(name,description, (int)user_id);
           var found_name = groupFactory.FindByName(name);
           var group_id = found_name.id;

            groupFactory.AddUserGroup((int) user_id, (int) group_id);
            return RedirectToAction("Group");
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Route("leave_group/{group_id}")]
        public IActionResult LeaveGroup(int group_id)
        {
            HttpContext.Session.SetInt32("user_id", 1);
            var user_id = HttpContext.Session.GetInt32("user_id");
        //   
            groupFactory.LeaveGroup((int)user_id, (int) group_id);
            return RedirectToAction("Group");
        }
        
    }
}

// DELETE FROM `belt_reviewer`.`groups` WHERE `id`='2';