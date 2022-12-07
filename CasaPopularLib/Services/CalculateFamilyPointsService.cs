using CasaPopularLib.Models;

namespace CasaPopularLib.Services
{
    public class CalculateFamilyPointsService : ICalculateFamilyPointsService
    {
        public virtual int CalculatePoints(Family family)
        {
            var totalFamilyIncome = GetTotalFamilyIncome(family);

            if (totalFamilyIncome <= 900)
            {
                return 5;
            }

            if (totalFamilyIncome >= 901 && totalFamilyIncome <= 1500)
            {
                return 3;
            }

            return 0;
        }

        private decimal GetTotalFamilyIncome(Family family)
        {
            return family.People.Sum(p => p.Salary);
        }
    }
}
