using CasaPopularLib.Models;

namespace CasaPopularLib.Services
{
    public class CalculateFamilyPointsWithDependentsService : CalculateFamilyPointsService, ICalculateFamilyPointsService
    {
        public override int CalculatePoints(Family family)
        {
            var dependents = family.People.Count(p => p.Age < 18);

            if (dependents >= 3)
            {
                return 3;
            }

            if (dependents == 1 || dependents == 2)
            {
                return 2;
            }

            return 0;
        }
    }
}
