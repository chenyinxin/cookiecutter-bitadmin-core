using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class GeneralExample
    {
        public Guid ExampleId { get; set; }
        public string ExampleName { get; set; }
        public string PlainText { get; set; }
        public string RequiredText { get; set; }
        public string RepeatText { get; set; }
        public string MobileText { get; set; }
        public string EmailText { get; set; }
        public int? PlainInt { get; set; }
        public decimal? PlainDecimal { get; set; }
        public DateTime? DateTimePicker { get; set; }
        public DateTime? DateTimePickerDate { get; set; }
        public DateTime? DateTimePickerYymm { get; set; }
        public DateTime? DateTimePickerFormatter { get; set; }
        public string UserPicker { get; set; }
        public string OuPicker { get; set; }
    }
}
