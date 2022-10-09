using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreProjectWeb.Models;
using PreProjectWeb.Models.ViewModel;
using PreProjectWeb.Repository;
using PreProjectWeb.Repository.IRepository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PreProjectWeb.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(StaticDetails.NationalParkAPIPath, HttpContext.Session.GetString("JWToken"));

            TrailsViewModel objVM = new TrailsViewModel()
            {
                NationalParkList = npList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                Trail = new Trail()
            };

            if (id == null)
            {
                //This will be true for Insert/Create
                return View(objVM);
            }

            //Flow will come here for update
            objVM.Trail = await _trailRepo.GetAsync(StaticDetails.TrailAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.Trail == null)
            {
                return NotFound();
            }
            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel obj)
        {
            if (ModelState.IsValid)
            {
                
                if (obj.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(StaticDetails.TrailAPIPath, obj.Trail, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _trailRepo.UpdateAsync(StaticDetails.TrailAPIPath + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(StaticDetails.NationalParkAPIPath, HttpContext.Session.GetString("JWToken"));

                TrailsViewModel objVM = new TrailsViewModel()
                {
                    NationalParkList = npList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()

                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(StaticDetails.TrailAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(StaticDetails.TrailAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
