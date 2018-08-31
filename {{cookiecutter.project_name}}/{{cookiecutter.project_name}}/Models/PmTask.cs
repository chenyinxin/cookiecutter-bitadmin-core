using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class PmTask
    {
        public Guid TaskId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? ParentId { get; set; }
        public string TaskType { get; set; }
        public string TaskName { get; set; }
        public string TaskContent { get; set; }
        public string TaskManager { get; set; }
        public string TaskStatus { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Remark { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
