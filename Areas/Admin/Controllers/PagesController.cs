﻿using CMSShoppingCART.Models.Data;
using CMSShoppingCART.Models.ViewModels.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CMSShoppingCART.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare List of Page Views (PageVM)
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                //Initialize the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //Return view with List

            return View(pagesList);
        }

        // Get: Admin/Pages/AddPage (Create a Page - Get)
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // Post: Admin/Pages/AddPage (Create a Page Post)

        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check the state of the model

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Decalre the slug to be used
                string slug;

                //initialize the DTO
                PageDTO dto = new PageDTO();

                //asign DTO title
                dto.Title = model.Title;

                //check and set slug
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //make the above unique
                if (db.Pages.Any(X => X.Title == model.Title) || db.Pages.Any(X => X.Slug == slug))
                {
                    ModelState.AddModelError("", "Title or Slug already exist");
                    return View(model);
                }

                //DTO evrything else

                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                dto.Sorting = 50;

                //Save DTO

                db.Pages.Add(dto);
                db.SaveChanges();

                //Set the temperorary Data Message
                TempData["SM"] = "New Page Added Successfully";

                //Redirect
                return RedirectToAction("AddPage");
            }
        }

        // Get: Admin/Pages/EditPage/id (Edit a Page Get)
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare pagesVM
            PageVM model;

            using (Db db = new Db())
            {
                //Get The Page
                PageDTO dto = db.Pages.Find(id);

                //Confirm that the page actually exists

                if (dto == null)
                {
                    return Content("This page doesn't exist. Sorry.");
                }

                //initialize page vm

                model = new PageVM(dto);

                //return the page with the model
                return View(model);
            }
        }

        [HttpPost]
        // Post: Admin/Pages/EditPage/id (Edit a Page Post)

        public ActionResult EditPage(PageVM model)
        {
            //Check Model State
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Set up Using for the db
            using (Db db = new Db())
            {
                //Get Page ID
                int id = model.Id;

                //Declare Slug
                string slug = "home";

                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Set the Title
                dto.Title = model.Title;

                //Check  for slug
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //Make sure it's unique

                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Dude, that title or the slug corresponding are already here. Give it a new one...");
                    return View(model);
                }

                //DTO for the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //Save the DTO
                db.SaveChanges();

                //Set the Temp Data
                TempData["SM"] = "New Edited the page Successfully";

                //Lastly redirect
                return RedirectToAction("EditPage");
            }
        }
    }
}