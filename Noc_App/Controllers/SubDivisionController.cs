using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using Noc_App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace Noc_App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SubDivisionController : Controller
    {
        private readonly IRepository<SubDivisionDetails> _repo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly IRepository<TehsilBlockDetails> _tehsilRepo;

        public SubDivisionController(IRepository<SubDivisionDetails> repo, IRepository<DivisionDetails> divisionRepo, IRepository<TehsilBlockDetails> tehsilRepo)
        {
            _repo = repo;
            _divisionRepo = divisionRepo;
            _tehsilRepo= tehsilRepo;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new SubDivisionViewModelCreate
            {
                Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name")
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SubDivisionViewModelCreate model)
        {
            try
            {
                model.Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name");
                if (ModelState.IsValid)
                {
                    string userid = LoggedInUserID();
                    bool IsDuplicate = _repo.IsDuplicateName(model.Name);
                    if (IsDuplicate)
                    {
                        ModelState.AddModelError("", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    SubDivisionDetails SubDivisionDetails = new SubDivisionDetails
                    {
                        Name = model.Name,
                        DivisionId=model.SelectedDivisionId,
                        IsActive=true,
                        CreatedOn= DateTime.Now,
                        CreatedBy = userid
                    };
                    await _repo.CreateAsync(SubDivisionDetails);
                    return RedirectToAction("List", "SubDivision");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult List()
        {
            try
            {
                IQueryable<SubDivisionDetails> list = _repo.GetAll().Include(x=>x.Division);
                var viewModels = list.Select(v => new SubDivisionViewModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    DivisionId = v.DivisionId,                    
                    DivisionName = v.Division.Name
                }).ToList();
                return View(viewModels);
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
                return View(await _repo.GetByIdAsync(id));
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
                SubDivisionDetails obj = await _repo.GetByIdAsync(Id);
                SubDivisionViewModelEdit model = new SubDivisionViewModelEdit
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    SelectedDivisionId = obj.DivisionId,
                    Divisions = new SelectList(_divisionRepo.GetAll(),"Id","Name",obj.DivisionId)
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
        public async Task<ActionResult> Edit(SubDivisionViewModelEdit model)
        {
            if (ModelState.IsValid)
            {
                SubDivisionDetails obj = await _repo.GetByIdAsync(model.Id);
                if (obj == null)
                {
                    ViewBag.ErrorMessage = $"Sub-Division with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    model.SelectedDivisionId = obj.DivisionId;
                    model.Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name", obj.DivisionId);
                    bool IsDuplicate = _repo.IsUniqueName(model.Name, model.Id);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid = LoggedInUserID();
                    obj.Name = model.Name;
                    obj.DivisionId = model.SelectedDivisionId;
                    obj.UpdatedOn = DateTime.Now;
                    obj.UpdatedBy = userid;
                    obj.IsActive=true;
                    await _repo.UpdateAsync(obj);
                    return RedirectToAction("List", "SubDivision");
                };


            }
            //PopulateDropdowns();
            return View(model);
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
        
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _repo.GetByIdAsync(id);
            obj.Division = await _divisionRepo.GetByIdAsync(obj.DivisionId);
            if (obj == null)
            {
                ViewBag.ErrorMessage = $"Sub-Division with Id = {obj.Id} cannot be found";
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
                ViewBag.ErrorMessage = $"Sub-Division with Id = {obj.Id} cannot be found";
                return View("NotFound");
            }
            //var users = await _userManager.Users.AnyAsync(x => x.SubDivisionId == id);
            var tehsil = await _tehsilRepo.FindAsync(x => x.SubDivisionId == id);
            if (/*users ||*/ tehsil.Count() > 0)
            {
                ModelState.AddModelError("e", $"Sub-Division {obj.Name} is already in use");
                return View(obj);
            }
            await _repo.DeleteAsync(id);

            return RedirectToAction("List","SubDivision");
        }
        private string LoggedInUserID()
        {
            string userId = HttpContext.Session.GetString("Userid");
            return userId;
        }

    } 
}
