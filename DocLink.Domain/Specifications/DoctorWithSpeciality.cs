using DocLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Specifications
{
	public class DoctorWithRelatedData : BaseSpecification<Doctor, string>
	{
		public DoctorWithRelatedData(string id, bool user = false, bool specialty = false, bool language = false, bool qualification = false) : base(D => D.Id == id)
		{
			if (user) Includes.Add(x => x.user);
			if (specialty) Includes.Add(x => x.Specialty);
			if (language) Includes.Add(x => x.Languages);
			if (qualification) Includes.Add(x => x.Languages);
			if (qualification) Includes.Add(x => x.Qualifications);
		}
	}
}
