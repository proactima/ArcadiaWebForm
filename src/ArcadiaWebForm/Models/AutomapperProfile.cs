using AutoMapper;

namespace ArcadiaWebForm.Models
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Opportunity.OpportunityInputModel, Opportunity.OpportunityOutputModel>()
                .ForMember(o => o.Client, opt => opt.ResolveUsing(c => new ArcadiaLink
                {
                    Type = "organisation",
                    Values = new[] { c.SelectedClient }
                }));
        }
    }
}
