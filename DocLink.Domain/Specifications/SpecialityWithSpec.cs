using DocLink.Domain.Entities;
namespace DocLink.Domain.Specifications
{
	public class SpecialtyWithSpec : BaseSpecification<Specialty, int>
	{
		public SpecialtyWithSpec(int Id = -1, string? name = null) : base(S =>
			(Id == -1 || S.Id == Id) &&
			(string.IsNullOrEmpty(name) || (S.Name == name))
		)
		{
			Includes.Add(x => x.Doctors);
		}
	}
}
