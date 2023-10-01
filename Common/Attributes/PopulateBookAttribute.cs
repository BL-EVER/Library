namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PopulateBookAttribute : Attribute
    {
        public string TargetPropertyName { get; }

        public PopulateBookAttribute(string targetPropertyName)
        {
            TargetPropertyName = targetPropertyName;
        }
    }
}
