using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cadastro.Domain.Entities;
using Cadastro.Infrastructure.Data.Common;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace Cadastro.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class ProductsController : Controller
    {
        private readonly RegisterContext _context;
        private readonly UserManager<User> _userManager;

        public ProductsController(RegisterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            var registerContext = _context.Products
                .Where(p => p.UserId == loggedUser.Id)
                .Include(p => p.Category);

            ViewBag.Reports =
                await _context.Reports
                .Include(x => x.Produtos).ThenInclude(x => x.Category)
                .ToListAsync();

            var avaliableProducts = await _context.Products
                .OrderBy(x=>x.Id)
                .Where(x => !x.IdRelatorio.HasValue).ToListAsync();

            return View(avaliableProducts);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> SaveReport(string name, Category category)
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            var produtos = await _context
                .Products
                .Where(x => !x.IdRelatorio.HasValue)
                .ToListAsync();

            var report = new Report(name, produtos, loggedUser.Id, category.Name);
            _context.Reports.Add(report);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Value,Active,IdCategory,Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.User = await _userManager.GetUserAsync(User);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name", product.IdCategory);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name", product.IdCategory);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Value,Active,IdCategory,Id")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name", product.IdCategory);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }



        [HttpPost]
        public IActionResult Export()
        {
            var contextProdutos = _context.Products.Include(p => p.Category);
            if (contextProdutos == null || !contextProdutos.Any())
            {
                return null;
            }
            var produtos = contextProdutos.ToList();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Código"),
                                        new DataColumn("Classe"),
                                        new DataColumn("Nome da classe") });


            foreach (var produto in produtos)
            {
                dt.Rows.Add(produto.Value, produto.Category.Name, produto.Name);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }

    }

}
