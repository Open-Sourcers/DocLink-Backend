using DocLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Specifications
{
	public class DoctorWithSpec : BaseSpecification<Doctor, string>
	{
		public DoctorWithSpec(DoctorParams param) : base(D =>
		(string.IsNullOrWhiteSpace(param.Name) || D.user.FirstName == param.Name) &&
		(string.IsNullOrWhiteSpace(param.SpecialtyName) || D.Specialty.Name == param.SpecialtyName)
		)
		{

			switch (param.Sort)
			{
				case Sort.OrderByRate:
					AddOrder(x => x.Rate);
					break;
				case Sort.OrderByRateDesc:
					AddOrderDescending(x => x.Rate);
					break;
				default:
					AddOrder(x => x.user.Id);
					break;

			}
			Includes.Add(x => x.Specialty);
			Includes.Add(x => x.user);

			int skip = (param.pageIndex - 1) * param.PageSize;
			skip = skip > 0 ? skip : 0;

			int take = param.PageSize;
			ApplayPagination(skip, take);
		}
	}
}
