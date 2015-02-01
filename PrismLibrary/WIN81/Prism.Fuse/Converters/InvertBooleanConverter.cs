namespace Microsoft.Practices.Prism.Converters
{
    public sealed class InvertBooleanConverter : BooleanConverter<bool>
    {
        public InvertBooleanConverter()
            : base(false, true)
        { }
    }
}
