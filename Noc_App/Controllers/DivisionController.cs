﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.Repository;
using Noc_App.Models.ViewModel;
using System.Xml.Linq;

namespace Noc_App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DivisionController : Controller
    {
        private readonly IRepository<DivisionDetails> _repo;
        private readonly IRepository<SubDivisionDetails> _repoSubdivision;

        public DivisionController(IRepository<DivisionDetails> repo, IRepository<SubDivisionDetails> repoSubdivision)
        {
            _repo = repo;
            _repoSubdivision = repoSubdivision;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DivisionViewModelCreate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userid = LoggedInUserID();

                    bool IsDuplicate = _repo.IsDuplicateName(model.Name);
                    if (IsDuplicate)
                    {
                        ModelState.AddModelError("", $"Name {model.Name} is already in use");
                        return View(model);
                    }

                    DivisionDetails divisionDetails = new DivisionDetails
                    {
                        Name = model.Name,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userid,
                        IsActive=true
                    };
                    await _repo.CreateAsync(divisionDetails);
                    return RedirectToAction("List", "Division");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            
            return View(model);
        }

        private string LoggedInUserID()
        {
            string userId = HttpContext.Session.GetString("Userid");
            return userId;
        }

        [HttpGet]
        public IActionResult List()
        {
            try
            {
                return View( _repo.GetAll());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public async Task<ViewResult> Details(int id)
        {
            try
            {
                return View( await _repo.GetByIdAsync(id));
                // return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> Edit(int Id)
        {
            try
            {
                DivisionDetails obj = await _repo.GetByIdAsync(Id);
                DivisionViewModelEdit model = new DivisionViewModelEdit
                {
                    Id = obj.Id,
                    Name = obj.Name
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }
        [HttpPost]
        public async Task<ActionResult> Edit(DivisionViewModelEdit model)
        {
            if (ModelState.IsValid)
            {
                DivisionDetails obj = await _repo.GetByIdAsync(model.Id);
                if (obj == null)
                {
                    ViewBag.ErrorMessage = $"Division with Id = {obj.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    bool IsDuplicate = _repo.IsUniqueName(model.Name,model.Id);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e",$"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid = LoggedInUserID();
                    obj.Name = model.Name;
                    obj.UpdatedOn = DateTime.Now;
                    obj.UpdatedBy = userid;
                    await _repo.UpdateAsync(obj);
                    return RedirectToAction("List", "Division");
                }


            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _repo.GetByIdAsync(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = $"Division with Id = {obj.Id} cannot be found";
                return View("NotFound");
            }

            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obj = await _repo.GetByIdAsync(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = $"Division with Id = {obj.Id} cannot be found";
                return View("NotFound");
            }

            //var users = await _userManager.Users.AnyAsync(x => x.DivisionId == id);
            var subdivision = await _repoSubdivision.FindAsync(x=>x.DivisionId==id);
            if (/*users ||*/ subdivision.Count()>0)
            {
                ModelState.AddModelError("e", $"Division {obj.Name} is already in use");
                return View(obj);
            }
            await _repo.DeleteAsync(id);

            return RedirectToAction("List", "Division");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult IsNameDuplicate(string name)
        {
            bool IsDuplicate = _repo.IsDuplicateName(name);

            if (IsDuplicate)
            {
                return Json(true);
            }
            else
            {
                return Json($"Name {name} is already in use");
            }
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult IsNameUnique(string name,int id)
        {
            bool IsDuplicate = _repo.IsUniqueName(name,id);

            if (IsDuplicate)
            {
                return Json(true);
            }
            else
            {
                return Json($"Name {name} is already in use");
            }
        }
    }
}
