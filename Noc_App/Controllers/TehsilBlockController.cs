using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using Noc_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Noc_App.Controllers
{
    [Authorize]
    public class TehsilBlockController : Controller
    {
        private readonly IRepository<TehsilBlockDetails> _repo;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public TehsilBlockController(IRepository<TehsilBlockDetails> repo, IRepository<SubDivisionDetails> subDivisionRepo, IRepository<DivisionDetails> divisionRepo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _subDivisionRepo = subDivisionRepo;
            _divisionRepo = divisionRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new TehsilBlockViewModelCreate
            {
                SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name")
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TehsilBlockViewModelCreate model)
        {
            try
            {
                model.Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name");
                var filteredSubdivisions = _subDivisionRepo.GetAll().Where(c => c.DivisionId == model.SelectedDivisionId).ToList();
                model.SubDivision = new SelectList(filteredSubdivisions, "Id", "Name");
                if (ModelState.IsValid)
                {
                    bool IsDuplicate = _repo.IsDuplicateName(model.Name);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid = await LoggedInUserID();
                    TehsilBlockDetails SaveModel = new TehsilBlockDetails
                    {
                        Name = model.Name,
                        SubDivisionId = model.SelectedSubDivisionId,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userid
                    };
                    await _repo.CreateAsync(SaveModel);
                    return RedirectToAction("List", "TehsilBlock");
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
                var list = _repo.GetAll().Include(x=>x.SubDivision);
                var viewModels = list.Select(v => new TehsilBlockViewModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    SubDivisionId = v.SubDivisionId,
                    SubDivisionName = v.SubDivision.Name,
                    DivisionId = v.SubDivision.DivisionId,
                    DivisionName = v.SubDivision.Division.Name,
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
                TehsilBlockDetails obj = await _repo.GetByIdAsync(Id);
                obj.SubDivision = await _subDivisionRepo.GetByIdAsync(obj.SubDivisionId);
                obj.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.SubDivision.DivisionId);
                TehsilBlockViewModelEdit model = new TehsilBlockViewModelEdit

                {
                    Id = obj.Id,
                    Name = obj.Name,
                    SelectedDivisionId = obj.SubDivision.DivisionId,
                    Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name", obj.SubDivision.DivisionId),
                    SelectedSubDivisionId= obj.SubDivisionId,
                    SubDivisions = new SelectList(_subDivisionRepo.GetAll().Where(x => x.DivisionId == obj.SubDivision.DivisionId), "Id", "Name", obj.SubDivisionId)
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
        public async Task<ActionResult> Edit(TehsilBlockViewModelEdit model)
        {
            if (ModelState.IsValid)
            {
                TehsilBlockDetails obj = await _repo.GetByIdAsync(model.Id);
                if (obj == null)
                {
                    ViewBag.ErrorMessage = $"Tehsil/Block with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    model.SubDivisions = new SelectList(_subDivisionRepo.GetAll().Where(x => x.DivisionId == model.SelectedDivisionId), "Id", "Name", model.SelectedSubDivisionId);
                    model.Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name", model.SelectedDivisionId);
                    //model.SelectedDivisionId = obj.SubDivision.DivisionId;
                    bool IsDuplicate = _repo.IsUniqueName(model.Name, model.Id);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid = await LoggedInUserID();
                    obj.Name = model.Name;
                    obj.SubDivisionId = model.SelectedSubDivisionId;
                    obj.UpdatedOn = DateTime.Now;
                    obj.UpdatedBy = userid;
                    obj.IsActive = true;
                    await _repo.UpdateAsync(obj);
                    return RedirectToAction("List", "TehsilBlock");
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
            obj.SubDivision = await _subDivisionRepo.GetByIdAsync(obj.SubDivisionId);
            obj.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.SubDivision.DivisionId);
            if (obj == null)
            {
                ViewBag.ErrorMessage = $"Tehsil/Block with Id = {obj.Id} cannot be found";
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
                ViewBag.ErrorMessage = $"Tehsil/Block with Id = {obj.Id} cannot be found";
                return View("NotFound");
            }

            await _repo.DeleteAsync(id);

            return RedirectToAction("List", "TehsilBlock");
        }
        private async Task<string> LoggedInUserID()
        {
            string userid = "0";
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _userManager.FindByNameAsync(user.UserName);

            if (user != null)
            {
                userid = user2.Id;
            }


            return userid;
        }

        [HttpPost]
        public IActionResult GetSubDivisions(int divisionId)
        {
            var subDivision =  _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }
    }
}
