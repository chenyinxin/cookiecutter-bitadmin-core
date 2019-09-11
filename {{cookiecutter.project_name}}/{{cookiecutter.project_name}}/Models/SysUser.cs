using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysUser
    {
        public Guid UserId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string IdCard { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Post { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string ExtendId { get; set; }
        public string UserImage { get; set; }
        public string UserStatus { get; set; }
        public int? OrderNo { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
