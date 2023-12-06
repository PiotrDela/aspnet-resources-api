namespace ResourcesManagementApi.Domain.Exceptions
{
    [Serializable]
	public class BusinessRuleValidationException : DomainException
	{
		public BusinessRuleValidationException() { }
		public BusinessRuleValidationException(string message) : base(message) { }
		public BusinessRuleValidationException(string message, Exception inner) : base(message, inner) { }
		protected BusinessRuleValidationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
