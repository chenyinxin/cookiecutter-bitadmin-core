using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowBillsRecord
    {
        public FlowBillsRecord()
        {
            FlowBillsRecordUser = new HashSet<FlowBillsRecordUser>();
        }

        public Guid Id { get; set; }
        public Guid BillsId { get; set; }
        public Guid? PrevStepId { get; set; }
        public Guid? NextStepId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PrevBillsRecordId { get; set; }
        public DateTime? AuditDate { get; set; }
        public int Sort { get; set; }
        public int Condition { get; set; }
        public string State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Choice { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int? Batch { get; set; }

        public FlowBills Bills { get; set; }
        public ICollection<FlowBillsRecordUser> FlowBillsRecordUser { get; set; }
    }
}
