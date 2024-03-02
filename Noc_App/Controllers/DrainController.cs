using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using Noc_App.Models.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Xml.Linq;
using System.Linq;

namespace Noc_App.Controllers
{
    [Authorize]
    public class DrainController : Controller
    {
        private readonly IRepository<DrainDetails> _repo;
        private readonly IRepository<DrainCoordinatesDetails> _repoLocation;
        private readonly UserManager<ApplicationUser> _userManager;

        public DrainController(IRepository<DrainDetails> repo, IRepository<DrainCoordinatesDetails> repoLocation, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
            _repoLocation = repoLocation;
        }
        public IActionResult Index()
        {
            try
            {
                var viewModel = _repo.GetAll();
                if (viewModel.Count() != 0)
                {
                    viewModel = viewModel.Include(d => d.DrainCoordinates);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                var viewModel = new DrainViewModelCreate
                {
                    DrainCoordinates = new List<DrainCoordinatesViewModelCreate> { new DrainCoordinatesViewModelCreate { Latitude = 0, Longitude = 0 } }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DrainViewModelCreate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userid = await LoggedInUserID();
                    var drainDetailsList = _repo.GetAll();

                    // Perform dynamic property access client-side
                    if(drainDetailsList.Any(d => EF.Property<string>(d, "Name") == model.Name))
                    {
                        ModelState.AddModelError("", $"Name {model.Name} is already in use");
                        return View(model);
                    }
                    List<DrainCoordinatesDetails> locationList = new List<DrainCoordinatesDetails>();
                    var coordinates = _repoLocation.GetAll();
                    foreach (var d in model.DrainCoordinates)
                    {
                        if (coordinates.Any(d => EF.Property<double>(d, "Latitude") == d.Latitude && EF.Property<double>(d, "Longitude") == d.Longitude)){
                            ModelState.AddModelError("", $"{d.Latitude} and {d.Longitude} is already in use");
                            return View(model);
                        }
                        DrainCoordinatesDetails location = new DrainCoordinatesDetails();
                        location.Latitude = d.Latitude;
                        location.Longitude = d.Longitude;
                        locationList.Add(location);
                    }
                    //if (obj.DrainCoordinates != null && obj.DrainCoordinates.Any())
                    //{
                        var duplicateCoordinates = model.DrainCoordinates
                        .GroupBy(c => new { c.Latitude, c.Longitude })
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();

                        if (duplicateCoordinates.Any())
                        {
                            foreach (var duplicate in duplicateCoordinates)
                            {
                                ModelState.AddModelError("", $"Duplicate coordinates found in a table- Latitude: {duplicate.Latitude}, Longitude: {duplicate.Longitude}.");
                            }

                            return View(model);
                        }

                        //var ext = _repoLocation.GetAll();
                        //List<DrainCoordinatesDetails> existingCoordinates=new List<DrainCoordinatesDetails>();
                        //if (ext.Count()> 0) {
                        //    existingCoordinates= ext.Where(c => obj.DrainCoordinates.Any(dc => dc.Latitude == c.Latitude && dc.Longitude == c.Longitude))
                        //.ToList(); 
                        //}

                        //if (existingCoordinates.Any())
                        //{
                        //    foreach (var duplicate in existingCoordinates)
                        //    {
                        //        ModelState.AddModelError("", $"Duplicate coordinates found in saved records- Latitude: {duplicate.Latitude}, Longitude: {duplicate.Longitude}.");
                        //    }

                        //    return View(obj);
                        //}

                        //foreach (var coordinate in obj.DrainCoordinates)
                        //{
                        //    coordinate.DrainId = obj.Id; // Assuming you have a foreign key in CoordinateModel for the Drain
                        //    coordinate.CreatedOn = DateTime.Now;
                        //    coordinate.CreatedBy = userid;
                        //    await _repoLocation.CreateAsync(coordinate);
                        //}
                    //}

                    DrainDetails obj = new DrainDetails
                    {
                        Name = model.Name,
                        DrainCoordinates = locationList,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userid
                    };
                    await _repo.CreateAsync(obj);

                    
                    return RedirectToAction("Index", "Drain");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        // DrainController.cs
        public async Task<ActionResult> Edit(int Id)
        {
            try
            {
                DrainDetails obj = await _repo.GetByIdAsync(Id);
                List<DrainCoordinatesViewModel> locationList = new List<DrainCoordinatesViewModel>();
                List<DrainCoordinatesDetails> dr = _repoLocation.GetAll().Where(x => x.DrainId == obj.Id).ToList();
                foreach (var d in dr)
                {
                    DrainCoordinatesViewModel location = new DrainCoordinatesViewModel();
                    location.Latitude = d.Latitude;
                    location.Longitude = d.Longitude;
                    location.Id=d.Id;
                    locationList.Add(location);
                }
                DrainViewModel model = new DrainViewModel
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    DrainCoordinates= locationList
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
        public async Task<ActionResult> Edit(DrainViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DrainDetails obj = await _repo.GetByIdAsync(model.Id);
                    if (obj == null)
                    {
                        ViewBag.ErrorMessage = $"Drain with Id = {obj.Id} cannot be found";
                        return View("NotFound");
                    }
                    else
                    {
                        var drainDetailsList = _repo.GetAll();

                        // Perform dynamic property access client-side
                        if (drainDetailsList.Any(d => EF.Property<string>(d, "Name") == model.Name && EF.Property<int>(d, "Id") != model.Id))
                        {
                            ModelState.AddModelError("", $"Name {model.Name} is already in use");
                            return View(model);
                        }
                        string userid = await LoggedInUserID();
                        List<DrainCoordinatesDetails> locationList = new List<DrainCoordinatesDetails>();
                        foreach (var d in model.DrainCoordinates)
                        {
                            DrainCoordinatesDetails objLocation = await _repoLocation.GetByIdAsync(d.Id);
                            //DrainCoordinatesDetails location = new DrainCoordinatesDetails();
                            objLocation.Id = d.Id;
                            objLocation.Latitude = d.Latitude;
                            objLocation.Longitude = d.Longitude;
                            objLocation.UpdatedBy = userid;
                            objLocation.UpdatedOn= DateTime.Now;
                            ///await _repoLocation.UpdateAsync(location);
                            locationList.Add(objLocation);
                        }
                        obj.Name = model.Name;
                        obj.DrainCoordinates = locationList;
                        obj.UpdatedOn = DateTime.Now;
                        obj.UpdatedBy = userid;
                        await _repo.UpdateAsync(obj);
                        return RedirectToAction("Index", "Drain");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle the exception, e.g., log it or display a user-friendly message
                ModelState.AddModelError("", "Error updating coordinates.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var drain = await _repo.GetByIdAsync(id);
                drain.DrainCoordinates = await _repoLocation.GetAll().Where(x => x.DrainId == drain.Id).ToListAsync();

                if (drain == null)
                {
                    ViewBag.ErrorMessage = $"Drain with Id = {drain.Id} cannot be found";
                    return View("NotFound");
                }

                if (drain.DrainCoordinates != null)
                {
                    ViewBag.ErrorMessage = $"Drain with Id = {drain.Id} cannot be deleted due already in use";
                    return View(drain);
                }

                return View(drain);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drain = await _repo.GetByIdAsync(id);
            drain.DrainCoordinates = await _repoLocation.GetAll().Where(x => x.DrainId == drain.Id).ToListAsync();

            if (drain == null)
            {
                ViewBag.ErrorMessage = $"Drain with Id = {drain.Id} cannot be found";
                return View("NotFound");
            }

            if (drain.DrainCoordinates != null)
            {
                ViewBag.ErrorMessage = $"Drain with Id = {drain.Id} cannot be deleted due already in use";
                return View(drain);
            }

            try
            {
                await _repo.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                // Handle the exception, e.g., log it or display a user-friendly message
                ModelState.AddModelError("", "Error deleting drain.");
                return View(drain);
            }
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

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult IsNameDuplicate(string name)
        {
            var drainDetailsList = _repo.GetAll();
            if (drainDetailsList.Any(d => EF.Property<string>(d, "Name") == name))
            {
                return Json($"Name {name} is already in use");
            }

            else
            {
                return Json(true);
            }
        }

        //[HttpPost]
        //public IActionResult AddMore(DrainViewModel viewModel)
        //{
        //        viewModel.DrainCoordinates.Add(new DrainCoordinatesViewModel()); // Add a new item to the list
        //    return PartialView("_DrainCoordinatesPartial", viewModel);
        //}
    }
}
