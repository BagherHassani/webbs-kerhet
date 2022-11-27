using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApencoded.Data;
using WebApencoded.Models;

namespace WebApencoded.Controllers
{
    public class EncoedDataEntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string[] _tagAllowed = new string[] { "<b>", "</b>", "<i>", "</i>", "<h2>", "</h2>" };
        public EncoedDataEntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EncoedDataEntities
        public async Task<IActionResult> Index()
        {
            List<EncoedDataEntity> Messages = await _context.Messages.ToListAsync();
            foreach (var message in Messages)
            {
                message.Description = HttpUtility.HtmlEncode(message.Description);

              
            }
            foreach (var tag in _tagAllowed)
            {
                var encodedTag = HttpUtility.HtmlEncode(tag);
                foreach (var message in Messages)
                {
                    message.Description = message.Description.Replace(encodedTag, tag);

                 
                }
            }

              return View(Messages);
        }

        // GET: EncoedDataEntities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var encoedDataEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encoedDataEntity == null)
            {
                return NotFound();
            }

            return View(encoedDataEntity);
        }

        // GET: EncoedDataEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EncoedDataEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] EncoedDataEntity encoedDataEntity)
        {
            if (ModelState.IsValid)
            {
                encoedDataEntity.Id = Guid.NewGuid();
                _context.Add(encoedDataEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(encoedDataEntity);
        }

        // GET: EncoedDataEntities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var encoedDataEntity = await _context.Messages.FindAsync(id);
            if (encoedDataEntity == null)
            {
                return NotFound();
            }
            return View(encoedDataEntity);
        }

        // POST: EncoedDataEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Description")] EncoedDataEntity encoedDataEntity)
        {
            if (ModelState.IsValid)
            {
                encoedDataEntity.Id = Guid.NewGuid();

                string encodedDescrip = HttpUtility.HtmlEncode(encoedDataEntity.Description);

                foreach (var tag in _tagAllowed)
                {
                    var encodedTag = HttpUtility.HtmlEncode(tag);
                    encodedDescrip = encodedDescrip.Replace(encodedTag, tag);


                }

                encoedDataEntity.Description = encodedDescrip;




                _context.Add(encoedDataEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(encoedDataEntity);
        }

        // GET: EncoedDataEntities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var encoedDataEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encoedDataEntity == null)
            {
                return NotFound();
            }

            return View(encoedDataEntity);
        }

        // POST: EncoedDataEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Messages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Messages'  is null.");
            }
            var encoedDataEntity = await _context.Messages.FindAsync(id);
            if (encoedDataEntity != null)
            {
                _context.Messages.Remove(encoedDataEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncoedDataEntityExists(Guid id)
        {
          return _context.Messages.Any(e => e.Id == id);
        }
    }
}
