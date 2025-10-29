using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services
{
    public class LeaveTypesService(ApplicationDbContext context, IMapper mapper) : ILeaveTypesService
    {
        public async Task<List<ReadOnlyVM>> GetAllAsync()
        {
            // SELECT * FROM LeaveTypes
            var data = await context.LeaveTypes.ToListAsync();
            // Connvert datamodel to viewmodel
            var viewData = mapper.Map<List<ReadOnlyVM>>(data);
            return viewData;
        }

        public async Task<T>? GetAsync<T>(Guid id) where T : class
        {
            var data = await context.LeaveTypes.FirstOrDefaultAsync(x => x.ID == id);
            if (data == null)
            {
                return null;
            }
            var viewData = mapper.Map<T>(data);
            return viewData;
        }

        public async Task Remove(Guid id)
        {
            var data = await context.LeaveTypes.FirstOrDefaultAsync(x => x.ID == id);
            if (data != null)
            {
                context.Remove(data);
                await context.SaveChangesAsync();
            }
        }

        public async Task Edit(EditVM model)
        {
            var leaveType = mapper.Map<LeaveType>(model);
            context.Update(leaveType);
            await context.SaveChangesAsync();
        }

        public async Task Create(CreateVM model)
        {
            var leaveType = mapper.Map<LeaveType>(model);
            context.Add(leaveType);
            await context.SaveChangesAsync();
        }

        public bool LeaveTypeExists(Guid id)
        {
            return context.LeaveTypes.Any(e => e.ID == id);
        }

        public async Task<bool> IfLeaveTypeExists(string name)
        {
            return await context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower()));
        }

        public async Task<bool> IfLeaveTypeExistsForEdit(EditVM editLeaveType)
        {
            return await context.LeaveTypes.AnyAsync(q => q.ID != editLeaveType.ID && q.Name.ToLower().Equals(editLeaveType.Name.ToLower()));
        }
    }
}
