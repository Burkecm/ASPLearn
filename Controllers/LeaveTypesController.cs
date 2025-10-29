using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using AutoMapper;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private const string LeaveTypeNameExistsMessage = "A Leave Type with that name already exists";

        public LeaveTypesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            // SELECT * FROM LeaveTypes
            var data = await _context.LeaveTypes.ToListAsync();
            // Connvert datamodel to viewmodel
            //var viewData = data.Select(q => new IndexVM {
            //    ID = q.ID,
            //    Name = q.Name,
            //    DaysAllocated = q.DaysAllocated,
            //});
            // returnb view model to view
            var viewData = mapper.Map<List<ReadOnlyVM>>(data);
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = mapper.Map<ReadOnlyVM>(leaveType);
            
            return View(viewData);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM leaveTypeCreate)
        {if (await IfLeaveTypeExists(leaveTypeCreate.Name))
                {
                    ModelState.AddModelError(nameof(leaveTypeCreate.Name), LeaveTypeNameExistsMessage);
                }
            if (ModelState.IsValid)
            {
                
                var leaveType = new LeaveType()
                {
                    Name = leaveTypeCreate.Name,
                    DaysAllocated = leaveTypeCreate.DaysAllocated,
                };
                leaveType.ID = Guid.NewGuid();
                _context.Add(leaveType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate);
        }


        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = mapper.Map<EditVM>(leaveType);
            return View(viewData);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditVM editLeaveType)
        {
            if (id != editLeaveType.ID)
            {
                return NotFound();
            }

            if (await IfLeaveTypeExistsForEdit(editLeaveType))
            {
                ModelState.AddModelError(nameof(editLeaveType.Name), LeaveTypeNameExistsMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var leaveType = mapper.Map<LeaveType>(editLeaveType);
                    _context.Update(leaveType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(editLeaveType.ID))
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
            return View(editLeaveType);
        }

        



        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = mapper.Map<ReadOnlyVM>(leaveType); 
            return View(viewData);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeExists(Guid id)
        {
            return _context.LeaveTypes.Any(e => e.ID == id);
        }
        
        private async Task<bool> IfLeaveTypeExists(string name)
        {
            return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower()));
        }

        private async Task<bool> IfLeaveTypeExistsForEdit(EditVM editLeaveType)
        {
            return await _context.LeaveTypes.AnyAsync(q => q.ID != editLeaveType.ID && q.Name.ToLower().Equals(editLeaveType.Name.ToLower()));
        }
    }
}
