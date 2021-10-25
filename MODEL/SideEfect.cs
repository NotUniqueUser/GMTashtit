using System;
namespace MODEL
{
    public class SideEfect : BaseEntity
    {
        private string name;
        public SideEfect()
        {
        }
        public SideEfect(string name)
        {
            this.name = name;
        }
        public string Name { get => name; set => name = value; }

        public override bool Equals(object obj)
        {
            return obj is SideEfect efect &&
                   name == efect.name;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}
