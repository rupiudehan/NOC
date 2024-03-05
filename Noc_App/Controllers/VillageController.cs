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
    public class VillageController : Controller
    {
        private readonly IRepository<VillageDetails> _repo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public VillageController(IRepository<VillageDetails> repo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo, IRepository<DivisionDetails> divisionRepo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _tehsilBlockRepo = tehsilBlockRepo;
            _subDivisionRepo = subDivisionRepo;
            _divisionRepo = divisionRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new VillageViewModelCreate
            {
                TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name")
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
                    model.Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name");
                    var filteredSubdivisions = _subDivisionRepo.GetAll().Where(c => c.DivisionId == model.SelectedDivisionId).ToList();
                    model.SubDivision = new SelectList(filteredSubdivisions, "Id", "Name");
                    var filteredtehsilBlock = _tehsilBlockRepo.GetAll().Where(c => c.SubDivisionId == model.SelectedSubDivisionId).ToList();
                    model.TehsilBlock = new SelectList(filteredtehsilBlock, "Id", "Name");

                    bool IsDuplicate = _repo.IsDuplicateName(model.Name);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid = await LoggedInUserID();
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
                var list = _repo.GetAll().Include(x=>x.TehsilBlock).ThenInclude(x=>x.SubDivision).ThenInclude(x=>x.Division);
                var viewModels = list.Select(v => new VillageViewModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    PinCode=v.PinCode,
                    TehsilBlockId = v.TehsilBlock.Id,
                    TehsilBlockName = v.TehsilBlock.Name,
                    SubDivisionId = v.TehsilBlock.SubDivision.Id,
                    SubDivisionName = v.TehsilBlock.SubDivision.Name,
                    DivisionId = v.TehsilBlock.SubDivision.Division.Id,
                    DivisionName = v.TehsilBlock.SubDivision.Division.Name,
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
                obj.TehsilBlock.SubDivision = await _subDivisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivisionId);
                obj.TehsilBlock.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivision.DivisionId);
                VillageViewModelEdit model = new VillageViewModelEdit

                {
                    Id = obj.Id,
                    Name = obj.Name,
                    PinCode = obj.PinCode,
                    SelectedTehsilBlockId = obj.TehsilBlock.Id,
                    TehsilBlock = new SelectList(_tehsilBlockRepo.GetAll().Where(x => x.SubDivisionId == obj.TehsilBlock.SubDivisionId), "Id", "Name", obj.TehsilBlockId),
                    SelectedSubDivisionId = obj.TehsilBlock.SubDivisionId,
                    SubDivisions = new SelectList(_subDivisionRepo.GetAll().Where(x=>x.DivisionId== obj.TehsilBlock.SubDivision.DivisionId), "Id", "Name", obj.TehsilBlock.SubDivisionId),
                    SelectedDivisionId = obj.TehsilBlock.SubDivision.DivisionId,
                    Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name", obj.TehsilBlock.SubDivision.DivisionId)
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
                    model.TehsilBlock = new SelectList(_tehsilBlockRepo.GetAll().Where(x => x.SubDivisionId == model.SelectedSubDivisionId), "Id", "Name", model.SelectedTehsilBlockId);
                    
                    model.SubDivisions = new SelectList(_subDivisionRepo.GetAll().Where(x => x.DivisionId == model.SelectedDivisionId), "Id", "Name", model.SelectedSubDivisionId);
                    model.Divisions = new SelectList(_divisionRepo.GetAll(), "Id", "Name", model.SelectedDivisionId);
                    

                    bool IsDuplicate = _repo.IsUniqueName(model.Name, model.Id);
                    if (IsDuplicate)
                    {
                        // Duplicate name found
                        ModelState.AddModelError("e", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    string userid = await LoggedInUserID();
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
            obj.TehsilBlock.SubDivision = await _subDivisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivisionId);
            obj.TehsilBlock.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivision.DivisionId);
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

            await _repo.DeleteAsync(id);

            return RedirectToAction("List", "Village");
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
            var subDivision = _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }

        [HttpPost]
        public IActionResult GetTehsilBlocks(int subDivisionId)
        {
            var tehsilBlock = _tehsilBlockRepo.GetAll();
            var filteredTehsilBlocks = tehsilBlock.Where(c => c.SubDivisionId == subDivisionId).ToList();
            return Json(new SelectList(filteredTehsilBlocks, "Id", "Name"));
        }
    }
}
