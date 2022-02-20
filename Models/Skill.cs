namespace Lancelittle.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Cost { get; set; }
        public CharClass Class { get; set; }
    }
}