using CasaPopularLib.Models;
using MediatR;

namespace CasaPopularLib.Handlers.FamilyGetPoints
{
    public class FamilyGetPointsQuery : IRequest<FamilyGetPointsResponse>
    {
        public Family Family { get; set; }

        internal bool HasDependents()
        {
            return Family.People.Count(p =>
                p.Kinship != Enums.KinshipType.CLAIMANT &&
                p.Kinship != Enums.KinshipType.SPOUSE &&
                p.Age < 18) > 0;
        }

        internal bool IsValid()
        {
            var validationResult = new FamilyGetPointsQueryValidator().Validate(this);

            return validationResult.IsValid;
        }
    }
}
