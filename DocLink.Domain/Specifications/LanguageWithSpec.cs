using DocLink.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Specifications
{
	public class LanguageWithSpec : BaseSpecification<LanguageSpoken, int>
	{
		public LanguageWithSpec(string lang) : base(L => L.Name == lang)
		{
			Includes.Add(x => x.Doctors);
		}
	}
}
