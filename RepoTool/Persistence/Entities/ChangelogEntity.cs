using RepoTool.Enums.Changelog;
using RepoTool.Persistence.Entities.Common;
using System.Text;

namespace RepoTool.Persistence.Entities
{
    public record Change
    {
        /// <summary>
        /// Description of the change
        /// </summary>
        public required string Description { get; set; }
        
        /// <summary>
        /// Reason for the change
        /// </summary>
        public required string Reason { get; set; }

        /// <summary>
        /// Importance rating of the change as determined by LLM
        /// </summary>
        public required EnChangeImportance Importance { get; set; }

        /// <summary>
        /// Quality rating of the change as determined by LLM
        /// </summary>
        public required EnChangeQuality Quality { get; set; }

        /// <summary>
        /// Functional area affected by the change
        /// </summary>
        public required EnChangeArea Area { get; set; }

        /// <summary>
        /// Type of the change
        /// </summary>
        public required EnChangeType Type { get; set; }

        /// <summary>
        /// Specialization area of the change
        /// </summary>
        public required EnChangeSpecialization Specialization { get; set; }

        /// <summary>
        /// Technical debt rating of the change
        /// </summary>
        public required EnChangeTechDebt TechnicalDebt { get; set; }

        /// <summary>
        /// Performance impact of the change
        /// </summary>
        public required EnChangePerformanceImpact PerformanceImpact { get; set; }

        /// <summary>
        /// Size of a change in a commit, rated against best-practice commit size
        /// </summary>
        public required EnChangeSize Size { get; set; } 

        public string FormatChange()
        {
            string importanceLabel = Importance switch
            {
                EnChangeImportance.Minor => "[MINOR]",
                EnChangeImportance.Normal => "[NORMAL]",
                EnChangeImportance.Major => "[MAJOR]",
                EnChangeImportance.Critical => "[CRITICAL]",
                _ => ""
            };

            string qualityLabel = Quality switch
            {
                EnChangeQuality.NeedsImprovement => "(⚠️ Needs Improvement)",
                EnChangeQuality.Acceptable => "(✔️ Acceptable)",
                EnChangeQuality.Good => "(✓ Good)",
                EnChangeQuality.Excellent => "(⭐ Excellent)",
                _ => ""
            };

            StringBuilder formatted = new();
            formatted.AppendLine($"- {Description}");
            formatted.AppendLine($"  - Importance: {importanceLabel}");
            formatted.AppendLine($"  - Quality: {qualityLabel}");
            formatted.AppendLine($"  - Area: {Area}");
            formatted.AppendLine($"  - Type: {Type}");
            formatted.AppendLine($"  - Specialization: {Specialization}");
            formatted.AppendLine($"  - Technical Debt: {TechnicalDebt}");
            formatted.AppendLine($"  - Performance Impact: {PerformanceImpact}");

            return formatted.ToString();
        }
    }

    public class ChangelogEntity : BaseEntity
    {
        /// <summary>
        /// List of changes in a commit
        /// </summary>
        public required List<Change> Changes { get; init; }
        
        /// <summary>
        /// Description of the commit, terms of new features, bug fixes, etc.
        /// </summary>
        public required string Description { get; init; }
        
        /// <summary>
        /// Reason for the commit, terms of new features, bug fixes, etc.
        /// </summary>
        public required string Reason { get; init; }

        public string FormatChanges() 
        {
            StringBuilder formatted = new();
            formatted.AppendLine("Changelog:");
            foreach (Change change in Changes)
            {
                formatted.AppendLine(change.FormatChange());
            }
            return formatted.ToString();
        }
    }
}