using CasaPopularLib.Services;
using MediatR;

namespace CasaPopularLib.Handlers.FamilyGetPoints
{
    public class FamilyGetPointsHandler : IRequestHandler<FamilyGetPointsQuery, FamilyGetPointsResponse>
    {
        public Task<FamilyGetPointsResponse> Handle(FamilyGetPointsQuery request, CancellationToken cancellationToken)
        {
            var result = new FamilyGetPointsResponse
            {
                IsValidToAnalyse = request.IsValid()
            };

            if (!result.IsValidToAnalyse) return Task.FromResult(result);

            ICalculateFamilyPointsService calculateService = new CalculateFamilyPointsService();

            if (request.HasDependents())
            {
                calculateService = new CalculateFamilyPointsWithDependentsService();
            }

            result.Points = calculateService.CalculatePoints(request.Family);

            return Task.FromResult(result);
        }
    }
}
