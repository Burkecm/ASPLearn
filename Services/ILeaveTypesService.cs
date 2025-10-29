using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Services
{
    public interface ILeaveTypesService
    {
        Task Create(CreateVM model);
        Task Edit(EditVM model);
        Task<T>? GetAsync<T>(Guid id) where T : class;
        Task<List<ReadOnlyVM>> GetAllAsync();
        Task Remove(Guid id);
        bool LeaveTypeExists(Guid id);
        Task<bool> IfLeaveTypeExists(string name);
        Task<bool> IfLeaveTypeExistsForEdit(EditVM editLeaveType);
    }
}