using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysModule
    {
        public Guid ModuleId { get; set; }
        public Guid? ParentId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public string Description { get; set; }
        public int? OrderNo { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
