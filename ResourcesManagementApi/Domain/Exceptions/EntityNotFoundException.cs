﻿namespace ResourcesManagementApi.Domain.Exceptions
{
    [Serializable]
	public class EntityNotFoundException : DomainException
	{
		public EntityNotFoundException() { }
		public EntityNotFoundException(string message) : base(message) { }
		public EntityNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected EntityNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
