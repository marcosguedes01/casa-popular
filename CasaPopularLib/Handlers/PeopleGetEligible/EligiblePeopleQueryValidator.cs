using CasaPopularLib.Models;
using FluentValidation;

namespace CasaPopularLib.Handlers.PeopleGetEligible
{
    internal sealed class EligiblePeopleQueryValidator : AbstractValidator<EligiblePeopleQuery>
    {
        public EligiblePeopleQueryValidator()
        {
            RuleFor(x => x.Families)
                .NotNull().WithMessage("Por favor, informe os dados dos familiares.")
                .NotEmpty().WithMessage("Por favor, informe os dados dos familiares.")
                .Must(p => CheckSameId(p)).WithMessage("Uma família pode ser listada apenas uma vez.");

        }

        private bool CheckSameId(IEnumerable<Family> familes)
        {
            var totalCount = familes.Count();
            var filterCount = familes.GroupBy(item => item.Id).Select(group => group.First()).Count();

            return totalCount == filterCount;
        }
    }
}
