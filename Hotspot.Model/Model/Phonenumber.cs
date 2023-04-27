namespace Hotspot.Model.Model
{
    public class Phonenumber
    {
        public Phonenumber(string number)
        {
            Number = number;
        }

        public int Id { get; set; }
        public string Number { get; set; }
    }
}