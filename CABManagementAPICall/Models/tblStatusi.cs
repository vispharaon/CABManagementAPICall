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
    
    public partial class tblStatusi
    {
        public tblStatusi()
        {
            this.tblCABComment = new HashSet<tblCABComment>();
            this.tblCABHistory = new HashSet<tblCABHistory>();
        }
    
        public int StatusID { get; set; }
        public string StatusDesc { get; set; }
        public Nullable<int> StatusForBusiness { get; set; }
    
        public virtual ICollection<tblCABComment> tblCABComment { get; set; }
        public virtual ICollection<tblCABHistory> tblCABHistory { get; set; }
    }
}
