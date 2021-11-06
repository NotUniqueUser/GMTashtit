using System.Collections.Generic;
using SQLite;

namespace MODEL
{
    [Table("Cities")]
    public class City : BaseEntity
    {
        public City()
        {
        }

        public City(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is City city &&
                   base.Equals(obj) &&
                   Name == city.Name;
        }

        public static bool operator ==(City left, City right)
        {
            return EqualityComparer<City>.Default.Equals(left, right);
        }

        public static bool operator !=(City left, City right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}