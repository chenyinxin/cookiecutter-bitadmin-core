using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysAttachment
    {
        public Guid Id { get; set; }
        public string RelationId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? Type { get; set; }
        public string Suffix { get; set; }
        public string Path { get; set; }
        public string Names { get; set; }
        public int? Status { get; set; }
        public long? Size { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
