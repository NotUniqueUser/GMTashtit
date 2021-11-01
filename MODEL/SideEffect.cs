using System;
namespace MODEL
{
    public class SideEffect : BaseEntity
    {
        private string name;
        public SideEffect()
        {
        }
        public SideEffect(string name)
        {
            this.name = name;
        }
        public string Name { get => name; set => name = value; }

        public override bool Equals(object obj)
        {
            return obj is SideEffect efect &&
                   name == efect.name;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}
