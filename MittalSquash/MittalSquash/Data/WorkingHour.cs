//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MittalSquash.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkingHour
    {
        public int ID { get; set; }
        public string WorkDay { get; set; }
        public int DayOfWeek { get; set; }
        public Nullable<int> StartTimeHour { get; set; }
        public Nullable<int> StartTimeMinute { get; set; }
        public Nullable<int> EndTimeHour { get; set; }
        public Nullable<int> EndTimeMinute { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}
