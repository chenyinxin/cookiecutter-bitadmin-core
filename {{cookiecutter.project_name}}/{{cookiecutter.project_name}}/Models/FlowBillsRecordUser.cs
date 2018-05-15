using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowBillsRecordUser
    {
        public Guid Id { get; set; }
        public Guid? StepId { get; set; }
        public Guid BillsRecordId { get; set; }
        public Guid? BillsRecordOutId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Type { get; set; }
        public string State { get; set; }
        public string Condition { get; set; }
        public string Choice { get; set; }
        public string Opinion { get; set; }
        public string DisplayState { get; set; }
        public DateTime? RunTime { get; set; }

        public FlowBillsRecord BillsRecord { get; set; }
    }
}
