using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;

namespace DocLink.Domain.Specifications
{
    public class PatientWithRelatedData : BaseSpecification<Patient, string>
    {
        public PatientWithRelatedData(string Id, bool user = false) : base(P => P.Id == Id)
        { 
            if (user) Includes.Add(P => P.user);
        }
    }
}
