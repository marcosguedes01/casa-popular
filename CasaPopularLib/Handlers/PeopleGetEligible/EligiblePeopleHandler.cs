using CasaPopularLib.Handlers.FamilyGetPoints;
using CasaPopularLib.Models;
using MediatR;

namespace CasaPopularLib.Handlers.PeopleGetEligible
{
    public class EligiblePeopleHandler : IRequestHandler<EligiblePeopleQuery, IEnumerable<EligiblePeopleResponse>>
    {
        private readonly IMediator _mediator;

        public EligiblePeopleHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IEnumerable<EligiblePeopleResponse>> Handle(EligiblePeopleQuery request, CancellationToken cancellationToken)
        {
            var result = new List<EligiblePeopleResponse>();

            if (!request.IsValid())
            {
                return Task.FromResult(result.AsEnumerable());
            }

            Parallel.ForEach(request.Families, async family =>
            {
                var familyPoints = await GetFamilyPointsAsync(family);
                var points = familyPoints.Points;

                if (familyPoints.IsValidToAnalyse)
                    result.Add(new EligiblePeopleResponse
                    {
                        FullName = family.FullName,
                        Points = points
                    });
            });

            IEnumerable<EligiblePeopleResponse> orderedResult = result.OrderByDescending(p => p.Points);

            return Task.FromResult(orderedResult);
        }

        private async Task<FamilyGetPointsResponse> GetFamilyPointsAsync(Family family)
        {
            var query = new FamilyGetPointsQuery { Family = family };
            return await _mediator.Send(query);
        }
    }
}
