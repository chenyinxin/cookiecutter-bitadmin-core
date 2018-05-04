using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysSmsCode
    {
        public Guid Id { get; set; }
        public string Mobile { get; set; }
        public string SmsSign { get; set; }
        public string SmsCode { get; set; }
        public int? IsVerify { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? OverTime { get; set; }
        public DateTime? VerifyTime { get; set; }
    }
}
