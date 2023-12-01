namespace TextSearchDemo.Models
{
    public class Child
    {
        public string Value { get; }
        public string PropertyName { get; }
        public int Weight { get; }
        public object Result { get; }
        public bool FullWord { get ; }

        public Child(string value, string propertyName, int weight, bool fullWord, object result)
        {
            Value = value;
            PropertyName = propertyName;
            Weight = weight;
            FullWord = fullWord;
            Result = result;
        }
    }
}
