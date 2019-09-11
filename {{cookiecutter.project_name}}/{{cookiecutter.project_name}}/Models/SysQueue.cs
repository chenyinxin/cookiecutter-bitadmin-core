using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysQueue
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ActionName { get; set; }
        public string ActionObjectId { get; set; }
        public string ActionObjectType { get; set; }
        public string ActionData { get; set; }
        public string ResultState { get; set; }
        public int? ResultNumber { get; set; }
        public DateTime? ResultTime { get; set; }
        public string ResultRemark { get; set; }
    }
}
