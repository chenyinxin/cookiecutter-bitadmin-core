using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysDictionary
    {
        public string Type { get; set; }
        public string Member { get; set; }
        public string MemberName { get; set; }
        public string Description { get; set; }
        public int? OrderNo { get; set; }
    }
}
