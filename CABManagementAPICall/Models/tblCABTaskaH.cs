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
    
    public partial class tblCABTaskaH
    {
        public int CAB_Task_ID { get; set; }
        public int CAB_HD_No { get; set; }
        public Nullable<int> CabHD_No_Task { get; set; }
        public string CAB_Task_Text { get; set; }
        public Nullable<System.DateTime> CAB_Task_Create_Date { get; set; }
        public Nullable<int> WorkingHours { get; set; }
        public string TaskerID { get; set; }
        public string TaskText { get; set; }
        public string DeveloperID { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
    }
}
