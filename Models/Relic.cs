namespace Lancelittle.Models
{
    public class Relic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public Character Character { get; set; }
        //One to one relationship requires the Foreign Key Id on the child
        public int CharacterId { get; set; }
    }
}