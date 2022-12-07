namespace CasaPopularLib.Models
{
    public sealed class Family
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public IEnumerable<FamilyPerson> People { get; set; }
    }
}
