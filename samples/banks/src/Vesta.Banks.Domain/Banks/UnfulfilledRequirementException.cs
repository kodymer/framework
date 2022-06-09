namespace Vesta.Banks
{
    [Serializable]
    public class UnfulfilledRequirementException : BusinessException
    {
        public UnfulfilledRequirementException()
        {
        }

        public UnfulfilledRequirementException(string message) : base(message)
        {
        }

        public UnfulfilledRequirementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}