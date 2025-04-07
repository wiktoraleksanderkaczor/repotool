using Json.Schema.Generation;

namespace RepoTool.Persistence.Entities.Common
{
    public class BaseEntity
    {
        /// <summary>
        /// Unique identifier for entity
        /// </summary>
        [JsonExclude]
        public int Id { get; set; }
        
        /// <summary>
        /// Date and time when entity was created
        /// </summary>
        [JsonExclude]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when entity was last modified 
        /// </summary>
        [JsonExclude]
        public DateTime LastModifiedAt { get; set; }
    }
}