using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysOperation
    {
        public Guid Id { get; set; }
        public string OperationSign { get; set; }
        public string OperationName { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? OrderNo { get; set; }
    }
}
