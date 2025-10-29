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
using LeaveManagementSystem.Web.Services;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController(ILeaveTypesService leaveTypesService) : Controller
    {
        private const string LeaveTypeNameExistsMessage = "A Leave Type with that name already exists";
        private readonly ILeaveTypesService service = leaveTypesService;


        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var viewData = await service.GetAllAsync();
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await service.GetAsync<ReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
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
        {
            if (await service.IfLeaveTypeExists(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), LeaveTypeNameExistsMessage);
            }
            if (ModelState.IsValid)
            {
                await service.Create(leaveTypeCreate);
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

            var leaveType = await service.GetAsync<EditVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
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

            if (await service.IfLeaveTypeExistsForEdit(editLeaveType))
            {
                ModelState.AddModelError(nameof(editLeaveType.Name), LeaveTypeNameExistsMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await service.Edit(editLeaveType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!service.LeaveTypeExists(editLeaveType.ID))
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

            var leaveType = await service.GetAsync<ReadOnlyVM>(id.Value);
            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            await service.Remove(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
