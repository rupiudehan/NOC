using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using Noc_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Noc_App.Controllers
{
    [Authorize(Roles = "Administrator,EXECUTIVE ENGINEER")]
    public class VillageController : Controller
    {
        private readonly IRepository<VillageDetails> _repo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        //private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        //private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly IRepository<DistrictDetails> _districtRepo;
        private readonly IRepository<GrantDetails> _grantRepo;

        public VillageController(IRepository<VillageDetails> repo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<GrantDetails> grantRepo
           , IRepository<DistrictDetails> districtRepo /*, IRepository<SubDivisionDetails> subDivisionRepo, IRepository<DivisionDetails> divisionRepo*/)
        {
            _repo = repo;
            _tehsilBlockRepo = tehsilBlockRepo;
            //_subDivisionRepo = subDivisionRepo;
            //_divisionRepo = divisionRepo;
            _districtRepo = districtRepo;
            _grantRepo = grantRepo;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new VillageViewModelCreate
            {
                TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                Districts = new SelectList(_districtRepo.GetAll().OrderBy(x => x.Name), "Id", "Name")
                //SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                //Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name")
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(VillageViewModelCreate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //model.Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name");
                    //var filteredSubdivisions = _subDivisionRepo.GetAll().Where(c => c.DivisionId == model.SelectedDivisionId).ToList();
                    model.Districts = new SelectList(_districtRepo.GetAll().OrderBy(x=>x.Name), "Id", "Name");
                    var filteredtehsilBlock = _tehsilBlockRepo.GetAll().Where(c => c.DistrictId == model.SelectedDistrictId).ToList();
                    model.TehsilBlock = new SelectList(filteredtehsilBlock, "Id", "Name");

                    bool IsDuplicate = _repo.IsDuplicateName(model.Name);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid = LoggedInUserID();
                    VillageDetails SaveModel = new VillageDetails
                    {
                        Name = model.Name,
                        PinCode = model.PinCode,
                        TehsilBlockId = model.SelectedTehsilBlockId,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userid
                    };
                    await _repo.CreateAsync(SaveModel);
                    return RedirectToAction("List", "Village");
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
                var list = _repo.GetAll().Include(x => x.TehsilBlock);//.ThenInclude(x=>x.SubDivision).ThenInclude(x=>x.Division);
                var viewModels = list.Select(v => new VillageViewModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    PinCode=v.PinCode,
                    TehsilBlockId = v.TehsilBlock.Id,
                    TehsilBlockName = v.TehsilBlock.Name,
                    //SubDivisionId = v.TehsilBlock.SubDivision.Id,
                    //SubDivisionName = v.TehsilBlock.SubDivision.Name,
                    DistrictId = v.TehsilBlock.District.Id,
                    DistrictName = v.TehsilBlock.District.Name,
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
                VillageDetails obj = await _repo.GetByIdAsync(Id);
                obj.TehsilBlock = await _tehsilBlockRepo.GetByIdAsync(obj.TehsilBlockId);
                obj.TehsilBlock.District = await _districtRepo.GetByIdAsync(obj.TehsilBlock.DistrictId);
                //obj.TehsilBlock.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivision.DivisionId);
                VillageViewModelEdit model = new VillageViewModelEdit

                {
                    Id = obj.Id,
                    Name = obj.Name,
                    PinCode = obj.PinCode,
                    SelectedTehsilBlockId = obj.TehsilBlock.Id,
                    TehsilBlock = new SelectList(_tehsilBlockRepo.GetAll().Where(x => x.DistrictId == obj.TehsilBlock.DistrictId).OrderBy(x=>x.Name), "Id", "Name", obj.TehsilBlockId),
                    SelectedDistrictId = obj.TehsilBlock.DistrictId,
                    Districts = new SelectList(_districtRepo.GetAll().OrderBy(x => x.Name), "Id", "Name", obj.TehsilBlock.DistrictId),
                    //SelectedDivisionId = obj.TehsilBlock.SubDivision.DivisionId,
                    //Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name", obj.TehsilBlock.SubDivision.DivisionId)
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
        public async Task<ActionResult> Edit(VillageViewModelEdit model)
        {
            if (ModelState.IsValid)
            {
                VillageDetails obj = await _repo.GetByIdAsync(model.Id);
                if (obj == null)
                {
                    ViewBag.ErrorMessage = $"Village with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    model.TehsilBlock = new SelectList(_tehsilBlockRepo.GetAll().Where(x => x.DistrictId == model.SelectedDistrictId), "Id", "Name", model.SelectedTehsilBlockId);
                    
                    //model.SubDivisions = new SelectList(_subDivisionRepo.GetAll().Where(x => x.DivisionId == model.SelectedDivisionId), "Id", "Name", model.SelectedSubDivisionId);
                    model.Districts = new SelectList(_districtRepo.GetAll().OrderBy(x=>x.Name), "Id", "Name", model.SelectedDistrictId);
                    

                    bool IsDuplicate = _repo.IsUniqueName(model.Name, model.Id);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid =  LoggedInUserID();
                    obj.Name = model.Name;
                    obj.PinCode=model.PinCode;
                    obj.TehsilBlockId = model.SelectedTehsilBlockId;
                    obj.UpdatedOn = DateTime.Now;
                    obj.UpdatedBy = userid;
                    obj.IsActive = true;
                    await _repo.UpdateAsync(obj);
                    return RedirectToAction("List", "Village");
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
            obj.TehsilBlock = await _tehsilBlockRepo.GetByIdAsync(obj.TehsilBlockId);
            obj.TehsilBlock.District = await _districtRepo.GetByIdAsync(obj.TehsilBlock.DistrictId);
            //obj.TehsilBlock.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivision.DivisionId);
            if (obj == null)
            {
                ViewBag.ErrorMessage = $"Village with Id = {obj.Id} cannot be found";
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
                ViewBag.ErrorMessage = $"Village with Id = {obj.Id} cannot be found";
                return View("NotFound");
            }

            //var users = await _userManager.Users.AnyAsync(x => x.SubDivisionId == id);
            var grant = await _grantRepo.FindAsync(x => x.VillageID == id);
            if (/*users ||*/ grant.Count() > 0)
            {
                ModelState.AddModelError("e", $"Village {obj.Name} is already in use");
                return View(obj);
            }
            await _repo.DeleteAsync(id);

            return RedirectToAction("List", "Village");
        }
        private string LoggedInUserID()
        {
            string userId = HttpContext.Session.GetString("Userid");
            return userId;
        }

        //[HttpPost]
        //public IActionResult GetSubDivisions(int divisionId)
        //{
        //    var subDivision = _subDivisionRepo.GetAll();
        //    var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
        //    return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        //}

        [HttpPost]
        public IActionResult GetTehsilBlocks(int subDivisionId)
        {
            var tehsilBlock = _tehsilBlockRepo.GetAll();
            var filteredTehsilBlocks = tehsilBlock.Where(c => c.DistrictId == subDivisionId).ToList();
            return Json(new SelectList(filteredTehsilBlocks, "Id", "Name"));
        }
    }
}
