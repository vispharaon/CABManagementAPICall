//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CABManagementAPICall.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CABTimes
    {
        public int CAB_HD_No { get; set; }
        public Nullable<int> ActualAnalyzeTime { get; set; }
        public Nullable<int> ActualTaskingTime { get; set; }
        public Nullable<System.DateTime> ScheduledTaskingStart { get; set; }
        public Nullable<System.DateTime> ScheduledTaskingEnd { get; set; }
        public Nullable<int> PredictedWHForTesting { get; set; }
    }
}
