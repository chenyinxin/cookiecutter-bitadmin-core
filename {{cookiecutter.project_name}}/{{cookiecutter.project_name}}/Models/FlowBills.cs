using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowBills
    {
        public FlowBills()
        {
            FlowBillsRecord = new HashSet<FlowBillsRecord>();
        }

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? MainId { get; set; }
        public Guid? StepId { get; set; }
        public string BillsType { get; set; }
        public string WorkOrderCode { get; set; }
        public string WorkOrderName { get; set; }
        public string BillsCode { get; set; }
        public int Sort { get; set; }
        public string State { get; set; }
        public Guid SubmitUser { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Description { get; set; }
        public int? Version { get; set; }

        public ICollection<FlowBillsRecord> FlowBillsRecord { get; set; }
    }
}
