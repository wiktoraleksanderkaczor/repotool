// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Persistence.Entities.Common
{
    internal abstract class BaseEntity
    {
        /// <summary>
        /// Unique identifier for entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time when entity was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when entity was last modified 
        /// </summary>
        public DateTime LastModifiedAt { get; set; }
    }
}
