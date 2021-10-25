using System;
namespace MODEL
{
    public class Clinic : BaseEntity
    {
        private string name;

        public Clinic()
        {
        }

        public Clinic(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            return obj is Clinic clinic &&
                   base.Equals(obj) &&
                   name == clinic.name;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}
