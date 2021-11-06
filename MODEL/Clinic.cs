namespace MODEL
{
    public class Clinic : BaseEntity
    {
        public Clinic()
        {
        }

        public Clinic(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Clinic clinic &&
                   base.Equals(obj) &&
                   Name == clinic.Name;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}