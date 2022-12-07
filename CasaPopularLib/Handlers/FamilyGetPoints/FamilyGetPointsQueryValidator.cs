using CasaPopularLib.Models;
using FluentValidation;

namespace CasaPopularLib.Handlers.FamilyGetPoints
{
    internal sealed class FamilyGetPointsQueryValidator : AbstractValidator<FamilyGetPointsQuery>
    {
        public FamilyGetPointsQueryValidator()
        {
            RuleFor(x => x.Family.People)
                .NotNull().WithMessage("Por favor, informe os dados dos familiares.")
                .NotEmpty().WithMessage("Por favor, informe os dados dos familiares.")
                .Must(p => HasOneClaimant(p)).WithMessage("Uma família pode ter apenas um pretendente.")
                .Must(p => HasOneSpouse(p)).WithMessage("Uma família pode ter apenas um cônjuge.");

        }

        private bool HasOneClaimant(IEnumerable<FamilyPerson> people)
        {
            return people.Count(p => p.Kinship == Enums.KinshipType.CLAIMANT) == 1;
        }

        private bool HasOneSpouse(IEnumerable<FamilyPerson> people)
        {
            return people.Count(p => p.Kinship == Enums.KinshipType.SPOUSE) <= 1;
        }
    }
}
