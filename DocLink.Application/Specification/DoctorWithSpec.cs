using DocLink.Domain.Entities;
using DocLink.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Application.Specification
{
	public class DoctorWithSpec : BaseSpecification<Doctor, string>
	{
		public DoctorWithSpec(DoctorParams param) : base(D =>
		string.IsNullOrWhiteSpace(param.Name) || D.user.FirstName == param.Name)
		{

			switch (param.Sort)
			{
				case Sort.OrderByName:
					AddOrder(x => x.user.FirstName);
					break;
				case Sort.OrderByNameDesc:
					AddOrderDescending(x => x.user.FirstName);
					break;
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

			int skip = (param.pageIndex - 1) * (param.PageSize);
			skip = skip > 0 ? skip : 0;

			int take = param.PageSize;
			ApplayPagination(skip, take);
		}
	}
}
