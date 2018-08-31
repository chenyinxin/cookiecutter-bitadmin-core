using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class PmProject
    {
        public Guid ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectType { get; set; }
        public string ProjectProperty { get; set; }
        public string ProjectName { get; set; }
        public string ProjectContent { get; set; }
        public Guid? ProjectManager { get; set; }
        public string ProjectManagerMobile { get; set; }
        public string ProjectStatus { get; set; }
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
