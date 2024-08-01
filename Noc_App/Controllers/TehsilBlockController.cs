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
    [Authorize(Roles = "Administrator,EXECUTIVE ENGINEER")]
    public class TehsilBlockController : Controller
    {
        private readonly IRepository<TehsilBlockDetails> _repo;
        //private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly IRepository<DistrictDetails> _districtRepo;
        //private readonly IRepository<VillageDetails> _villageRepo;
        private readonly IRepository<GrantDetails> _repoGrant;
        public TehsilBlockController(IRepository<TehsilBlockDetails> repo, /*IRepository<SubDivisionDetails> subDivisionRepo, */IRepository<DivisionDetails> divisionRepo, 
            /*IRepository<VillageDetails> villageRepo, */IRepository<DistrictDetails> districtRepo, IRepository<GrantDetails> repoGrant)
        {
            _repo = repo;
            //_subDivisionRepo = subDivisionRepo;
            _divisionRepo = divisionRepo;
            //_villageRepo = villageRepo;
            _districtRepo = districtRepo;
            _repoGrant = repoGrant;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new TehsilBlockViewModelCreate
            {
                Divisions = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                Districts = new SelectList(_districtRepo.GetAll().OrderBy(x=>x.Name), "Id", "Name")
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TehsilBlockViewModelCreate model)
        {
            try
            {
                model.Districts = new SelectList(_districtRepo.GetAll(), "Id", "Name");
                var filtereddivisions = await _divisionRepo.FindAsync(c => c.DistrictId == model.SelectedDistrictId);
                model.Divisions = new SelectList(filtereddivisions, "Id", "Name");
                if (ModelState.IsValid)
                {
                    //bool IsDuplicate = _repo.IsDuplicateName(model.Name);
                    //if (IsDuplicate)
                    //{
                    //    // Duplicate name found
                    //    ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                    //    return View(model);
                    //}
                    string userid = LoggedInUserID();
                    TehsilBlockDetails SaveModel = new TehsilBlockDetails
                    {
                        Name = model.Name,
                        DivisiontId = model.SelectedDivisionId,
                        IsActive = true,
                        LGD_ID=model.LGD_ID,
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
                var list = _repo.GetAll().Include(x => x.Divisions).Include(x => x.Divisions.District);
                var viewModels = list.Select(v => new TehsilBlockViewModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    LGD_ID=v.LGD_ID,
                    //SubDivisionId = v.SubDivisionId,
                    //SubDivisionName = v.SubDivision.Name,
                    DistrictId = v.Divisions.District.Id,
                    DistrictName = v.Divisions.District.Name,
                    DivisionId = v.Divisions.Id,
                    DivisionName = v.Divisions.Name
                }).OrderBy(x=>x.DistrictName).ThenBy(x=>x.DivisionName).ThenBy(x=>x.Name).ToList();
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
                
                obj.Divisions = await _divisionRepo.GetByIdAsync(obj.DivisiontId);
                var filtereddivisions = await _divisionRepo.FindAsync(c => c.DistrictId == obj.Divisions.DistrictId);
                
                //obj.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.SubDivision.DivisionId);
                TehsilBlockViewModelEdit model = new TehsilBlockViewModelEdit

                {
                    Id = obj.Id,
                    Name = obj.Name,
                    LGD_ID=obj.LGD_ID,
                    SelectedDistrictId = obj.Divisions.DistrictId,
                    Districts = new SelectList(_districtRepo.GetAll(), "Id", "Name", obj.Divisions.DistrictId),
                    Divisions = new SelectList(filtereddivisions, "Id", "Name",obj.DivisiontId),
                    SelectedDivisionId=obj.DivisiontId
                //SelectedSubDivisionId= obj.SubDivisionId,
                //SubDivisions = new SelectList(await _subDivisionRepo.FindAsync(x => x.DivisionId == obj.SubDivision.DivisionId), "Id", "Name", obj.SubDivisionId)
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
            model.Districts = new SelectList(_districtRepo.GetAll().OrderBy(x => x.Name), "Id", "Name", model.SelectedDistrictId);

            var filtereddivisions = await _divisionRepo.FindAsync(c => c.DistrictId == model.SelectedDistrictId);
            model.Divisions = new SelectList(filtereddivisions.OrderBy(x => x.Name), "Id", "Name", model.SelectedDivisionId);

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

                    //bool IsDuplicate = _repo.IsUniqueName(model.Name, model.Id);
                    //if (IsDuplicate)
                    //{
                    //    // Duplicate name found
                    //    ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                    //    return View(model);
                    //}
                    string userid = LoggedInUserID();
                    obj.Name = model.Name;
                    obj.LGD_ID= model.LGD_ID;
                    //obj.DistrictId = model.SelectedDistrictId;
                    obj.DivisiontId = model.SelectedDivisionId;
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
            if (obj == null)
            {
                ViewBag.ErrorMessage = $"Tehsil/Block with Id = {obj.Id} cannot be found";
                return View("NotFound");
            }
            obj.Divisions.District = await _districtRepo.GetByIdAsync(obj.Divisions.DistrictId);
            
            obj.Divisions = await _divisionRepo.GetByIdAsync(obj.DivisiontId);

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

            //var users = await _userManager.Users.AnyAsync(x => x.SubDivisionId == id);
            var tehsil = await _repoGrant.FindAsync(x => x.TehsilID == id);
            if (/*users || */tehsil.Count() > 0)
            {
                ModelState.AddModelError("e", $"Tehsil {obj.Name} is already in use");
                return View(obj);
            }
            await _repo.DeleteAsync(id);

            return RedirectToAction("List", "TehsilBlock");
        }
        private string LoggedInUserID()
        {
            string userId = HttpContext.Session.GetString("Userid");
            return userId;
        }

        [HttpPost]
        public IActionResult GetDivisions(int districtId)
        {
            var division = _divisionRepo.GetAll();
            var filtereddivisions = division.Where(c => c.DistrictId == districtId).ToList();
            return Json(new SelectList(filtereddivisions, "Id", "Name"));
        }
    }
}
