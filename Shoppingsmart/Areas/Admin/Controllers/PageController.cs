using Shoppingsmart.Models.Data;
using Shoppingsmart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppingsmart.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        // GET: Admin/Page
        public ActionResult Index()
        {
            List<PageVM> pagelist;
            using (Db db = new Db())
            {
                pagelist = db.pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            return View(pagelist);
        }

        // GET: Admin/Page/Addpage
        [HttpGet]
        public ActionResult Addpage()
        {
            return View();
        }
        // post: Admin/Page/Addpage
        [HttpPost]
        public ActionResult Addpage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //declare slug
                string slug;
                //init pageDTO
                PageDto dto = new PageDto();
                //DTO Title
                dto.Title = model.Title;
                //Check for and set slug if need
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace("@", "").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace("@", "").ToLower();
                }
                //make sure title and slug are unique
                if (db.pages.Any(x => x.Title == model.Title) || db.pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "slug and title already exists.");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Title = model.Title;
                dto.Body = model.Body;
                dto.Hassidebar = model.Hassidebar;
                dto.Sorting = 100;
                //save DTO
                db.pages.Add(dto);
                db.SaveChanges();
            }
            //set tempdata msg
            TempData["SM"] = "you have added a new page!";

            //redirect
            return RedirectToAction("Addpage");
            //  return View();
        }
        // GET: Admin/Page/Editpage/id
        [HttpGet]
        public ActionResult Editpage(int id)
        {
            //declare pagevm
            PageVM model;
            using (Db db = new Db())
            {
                //Get the page
                PageDto dto = db.pages.Find(id);
                //confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exists.");
                }
                //init pagevm
                model = new PageVM(dto);
            }
            return View(model);
        }

        //post: Admin/Page/Addpage/id
        [HttpPost]
        public ActionResult Editpage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //Get page id
                int id = model.Id;
                //declare slug
                string slug = "home";
                //get the page
                PageDto dto = db.pages.Find(id);
                //Dto the title
                dto.Title = model.Title;
                //check for the slug and set it to be
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace("+", "").ToLower();
                    }
                    else
                    {
                        slug = model.Title.Replace("+", "").ToLower();
                    }
                }
                //make sure title and slug for the unique
                if (db.pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || db.pages.Where(x => x.Id != id).Any(x => x.Slug == model.Title))
                {
                    ModelState.AddModelError("", "the slug or title already exists.");
                    return View(model);
                }
                //Dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.Hassidebar = model.Hassidebar;
                //save the page
                db.SaveChanges();

            }
            TempData["SM"] = "you have edited the page!.";
            return RedirectToAction("Editpage");
        }
        //Get/admin/page/PageDetails/id

        public ActionResult PageDetails(int id)
        {
            //declare pagevm
            PageVM model;
            using (Db db = new Db())
            {
                //get the page
                PageDto dto = db.pages.Find(id);
                //confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exists.");

                }
                //init pagevm
                model = new PageVM(dto);
            }
            return View(model);
        }
        //Get/admin/page/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //Get the page
                PageDto dto = db.pages.Find(id);
                //remove the page
                db.pages.Remove(dto);
                //save
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        //Get/Admin/page/Reorderpages
        [HttpPost]
        public void Reorderpages(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;
                //declare pagedto
                PageDto dto;
                //set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }
        public ActionResult EditSidebar()
        {
            //Declare model
            SidebarVM model;
            using (Db db = new Db())
            {
                //Get the Dto
                SideDto dto = db.Sidebar.Find(1);
                //Init model
                model = new SidebarVM(dto);
            }
            //return view with model
            return View(model);
        }
    }
}
