using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Data;
using PizzaStore.Models;

namespace PizzaStore.Controllers
{
    public class PizzaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PizzaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pizza
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pizzas.ToListAsync());
        }

        // GET: Pizza/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizzas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        [Authorize]
        public IActionResult Buy(int id)
        {
            var pizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }
      

    }

}
