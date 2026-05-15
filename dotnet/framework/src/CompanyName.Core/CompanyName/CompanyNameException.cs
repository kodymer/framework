namespace CompanyName
{
    [Serializable]
    public class CompanyNameException : Exception
    {
        public CompanyNameException()
        {
        }

        public CompanyNameException(string message) : base(message)
        {
        }

        public CompanyNameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}