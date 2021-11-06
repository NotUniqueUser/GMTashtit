namespace MODEL
{
    public class SideEffect : BaseEntity
    {
        public SideEffect()
        {
        }

        public SideEffect(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SideEffect efect &&
                   Name == efect.Name;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}