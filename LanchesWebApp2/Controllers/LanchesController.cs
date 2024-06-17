using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LanchesWebApp2.Data;
using LanchesWebApp2.Models;

namespace LanchesWebApp2.Controllers
{
    public class LanchesController : Controller
    {
        private readonly DbBitesContext _context;

        public LanchesController(DbBitesContext context)
        {
            _context = context;
        }

        // GET: Lanches
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lanches.ToListAsync());
        }

        // GET: Lanches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var lanche = await _context.Lanches
                .Include(l => l.LancheIngredientes)
                .ThenInclude(li => li.Ingrediente)
                .FirstOrDefaultAsync(m => m.LancheId == id);

            if (lanche == null)

            {

                return NotFound();

            }
            // Criar uma lista de nomes de ingredientes
            var ingredienteNames = lanche.LancheIngredientes.Select(li => li.Ingrediente.Nome).ToList();
            // Passar a lista para a view
            ViewBag.IngredienteNames = ingredienteNames;
            return View(lanche);
        }

        // GET: Lanches/Create
        public IActionResult Create()
        {
            ViewBag.Ingredientes = _context.Ingredientes.ToList();
            return View();
        }

        // POST: Lanches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LancheId,Nome,Descricao,Preco,IngredienteIds")] Lanche lanche)
        {
            ViewBag.Ingredientes = _context.Ingredientes.ToList();
            if (ModelState.IsValid)
            {
                _context.Add(lanche);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                foreach (var ingredienteId in lanche.IngredienteIds)
                {
                    var lancheIngrediente = new LancheIngrediente
                    {
                        LancheId = lanche.LancheId,
                        IngredienteId = ingredienteId
                    };
                    _context.LancheIngredientes.Add(lancheIngrediente);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(lanche);
        }

        // GET: Lanches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Ingredientes = _context.Ingredientes.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var lanche = await _context.Lanches.FindAsync(id);
            if (lanche == null)
            {
                return NotFound();
            }
            return View(lanche);
        }

        // POST: Lanches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LancheId,Nome,Descricao,Preco,IngredienteIds")] Lanche lanche)
        {
            if (id != lanche.LancheId)

            {
                return NotFound();
            }
            ViewBag.Ingredientes = _context.Ingredientes.ToList();
            if (ModelState.IsValid)
            {
                // Remover os registros de LancheIngrediente que não estão mais associados ao lanche
                var lancheIngredientesToRemove = _context.LancheIngredientes.Where(li => li.LancheId == lanche.LancheId && !lanche.IngredienteIds.Contains(li.IngredienteId));
                _context.LancheIngredientes.RemoveRange(lancheIngredientesToRemove);

                // Adicionar os novos registros de LancheIngrediente
                foreach (var ingredienteId in lanche.IngredienteIds)
                {
                    var lancheIngrediente = _context.LancheIngredientes.FirstOrDefault(li => li.LancheId == lanche.LancheId && li.IngredienteId == ingredienteId);
                    if (lancheIngrediente == null)
                    {
                        // Não existe, então adicionar um novo registro
                        lancheIngrediente = new LancheIngrediente
                        {
                            LancheId = lanche.LancheId,
                            IngredienteId = ingredienteId
                        };
                        _context.LancheIngredientes.Add(lancheIngrediente);
                    }

                }
                try
                {
                    _context.Update(lanche);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LancheExists(lanche.LancheId))
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
            return View(lanche);
        }

        // GET: Lanches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lanche = await _context.Lanches
                .Include(l => l.LancheIngredientes)
                .ThenInclude(li => li.Ingrediente)
                .FirstOrDefaultAsync(m => m.LancheId == id);

            if (lanche == null)
            {
                return NotFound();
            }

            // Criar uma lista de nomes de ingredientes
            var ingredienteNames = lanche.LancheIngredientes.Select(li => li.Ingrediente.Nome).ToList();
            // Passar a lista para a view
            ViewBag.IngredienteNames = ingredienteNames;
            return View(lanche);
        }

        // POST: Lanches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Excluir as referências do lanche em LancheIngredientes
            _context.LancheIngredientes.RemoveRange(_context.LancheIngredientes.Where(li => li.LancheId == id));

            var lanche = await _context.Lanches.FindAsync(id);
            if (lanche != null)
            {
                _context.Lanches.Remove(lanche);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LancheExists(int id)
        {
            return _context.Lanches.Any(e => e.LancheId == id);
        }
    }
}
