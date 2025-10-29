using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class ReadOnlyVM : BaseLeaveTypeVM
    {
        public string Name { get; set; } = string.Empty;
        [Display(Name="Number of Days Allocated")]
        public int DaysAllocated { get; set; }
    }
}
