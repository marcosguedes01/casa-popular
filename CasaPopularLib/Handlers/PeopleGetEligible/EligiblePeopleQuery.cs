using CasaPopularLib.Handlers.FamilyGetPoints;
using CasaPopularLib.Models;
using MediatR;

namespace CasaPopularLib.Handlers.PeopleGetEligible
{
    public class EligiblePeopleQuery : IRequest<IEnumerable<EligiblePeopleResponse>>
    {
        public List<Family> Families { get; set; }

        internal bool IsValid()
        {
            var validationResult = new EligiblePeopleQueryValidator().Validate(this);

            return validationResult.IsValid;
        }
    }
}
