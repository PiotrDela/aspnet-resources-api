﻿namespace ResourcesManagementApi.Domain.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public byte[] Version { get; set; }
    }
}
