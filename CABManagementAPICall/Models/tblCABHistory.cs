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
    
    public partial class tblCABHistory
    {
        public int CABStatusID { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
        public int StatusID { get; set; }
        public int AnalyzeID { get; set; }
        public int CAB_HD_No { get; set; }
    
        public virtual tblCAB tblCAB { get; set; }
        public virtual tblCABAnalysis tblCABAnalysis { get; set; }
        public virtual tblStatusi tblStatusi { get; set; }
    }
}
