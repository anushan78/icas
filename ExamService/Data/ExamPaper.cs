//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExamService.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExamPaper
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int GradeId { get; set; }
        public string Year { get; set; }
        public bool HasAnswers { get; set; }
        public string FileStoreId { get; set; }
    
        public virtual Grade Grade { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
