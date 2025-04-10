namespace RepoTool.Persistence.Entities.Common
{
    public class BaseEntity
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