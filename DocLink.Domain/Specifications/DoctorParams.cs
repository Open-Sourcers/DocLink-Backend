using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocLink.Domain.Specifications
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Sort
	{
		OrderByRate,
		OrderByRateDesc,
	}
	public class DoctorParams
	{
		public string? Name { get; set; }
		public Sort? Sort { get; set; }
		public string? SpecialtyName { get; set; }
		public int pageIndex { get; set; }
		private int pageSize = 20;
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > 100 || pageSize < 1 ? 20 : value; }
		}
	}

}
