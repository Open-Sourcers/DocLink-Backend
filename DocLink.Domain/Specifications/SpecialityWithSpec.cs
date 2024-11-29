using DocLink.Domain.Entities;
namespace DocLink.Domain.Specifications
{
	public class SpecialtyWithSpec:BaseSpecification<Specialty,int>
	{
        public SpecialtyWithSpec(int Id)
            :base(S=>S.Id==Id)
        {
            Includes.Add(x => x.Doctors);
        }
    }
}
