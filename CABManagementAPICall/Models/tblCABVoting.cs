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
    
    public partial class tblCABVoting
    {
        public string VoterID { get; set; }
        public int CAB_HD_No { get; set; }
        public int VoteID { get; set; }
        public Nullable<int> CABVote { get; set; }
        public Nullable<System.DateTime> CABVoteDate { get; set; }
    
        public virtual tblDevelopers tblDevelopers { get; set; }
    }
}
