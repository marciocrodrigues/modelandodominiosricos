namespace NerdStore.WebApp.API.Extensions
{
    public class Errors
    {
        public Errors(string key,  string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; private set; }

        public string Value { get; private set; }
    }
}
