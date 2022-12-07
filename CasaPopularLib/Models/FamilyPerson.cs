using CasaPopularLib.Enums;

namespace CasaPopularLib.Models
{
    public sealed class FamilyPerson
    {
        public KinshipType? Kinship { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
    }
}
