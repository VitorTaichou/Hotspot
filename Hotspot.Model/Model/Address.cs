namespace Hotspot.Model.Model
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }

        public virtual Locale Locale { get; set; }
    }
}