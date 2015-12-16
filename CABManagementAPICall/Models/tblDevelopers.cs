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
    
    public partial class tblDevelopers
    {
        public tblDevelopers()
        {
            this.tblCABAnalysis = new HashSet<tblCABAnalysis>();
            this.tblCABComment = new HashSet<tblCABComment>();
            this.tblCABTaska = new HashSet<tblCABTaska>();
            this.tblCABTaska1 = new HashSet<tblCABTaska>();
            this.tblCABVoting = new HashSet<tblCABVoting>();
        }
    
        public string DeveloperID { get; set; }
        public string Node { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Nullable<int> IsVoter { get; set; }
        public Nullable<int> IsAnalyzer { get; set; }
        public Nullable<int> IsTasker { get; set; }
        public Nullable<int> IsDeveloper { get; set; }
        public Nullable<int> IsTester { get; set; }
    
        public virtual ICollection<tblCABAnalysis> tblCABAnalysis { get; set; }
        public virtual ICollection<tblCABComment> tblCABComment { get; set; }
        public virtual ICollection<tblCABTaska> tblCABTaska { get; set; }
        public virtual ICollection<tblCABTaska> tblCABTaska1 { get; set; }
        public virtual ICollection<tblCABVoting> tblCABVoting { get; set; }
    }
}
