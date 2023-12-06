namespace ResourcesManagementApi.Domain.Exceptions
{

    [Serializable]
	public abstract class DomainException : Exception
	{
		public DomainException() { }
		public DomainException(string message) : base(message) { }
		public DomainException(string message, Exception inner) : base(message, inner) { }
		protected DomainException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
