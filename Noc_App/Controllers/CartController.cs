using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noc_App.Context;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.interfaces;

namespace Noc_App.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly IRepository<GrantDetails> _context;
        //private readonly ApplicationDbContext _context;
        public CartController(IRepository<GrantDetails> context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            try
            {
                var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;
                if (cart != null)
                {
                    ViewBag.total = cart.Sum(item => item.Grant.Khasras.Select(x => x.MarlaOrBiswansi).FirstOrDefault() * item.Quantity);
                }
                else { ViewBag.total = "0"; }
            }
            catch (Exception)
            {

                ViewBag.total = 0;
            }

            return View();
        }
        private int IsExist(int Id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Grant.Id.Equals(Id))
                {
                    return i;
                }
            }
            return -1;
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.GetByIdAsync(id??0);
            if (pet == null)
            {
                return NotFound();
            }
            return View(pet);
        }
        public async Task<IActionResult> Buy(int Id)
        {

            //    ProductModel productModel = new ProductModel();

            var pet = _context.GetByIdAsync(Id);

            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Grant = await _context.GetByIdAsync(Id), Quantity = 1 });
                SessionHelper.SetObjectasJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = IsExist(Id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Grant = await _context.GetByIdAsync(Id), Quantity = 1 });
                }
                SessionHelper.SetObjectasJson(HttpContext.Session, "cart", cart);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int Id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = IsExist(Id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectasJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");



        }
    }
}
