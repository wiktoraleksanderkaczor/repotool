namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the specialization area of a change
    /// </summary>
    public enum EnChangeSpecialization
    {
        /// <summary>
        /// Changes to frontend code or user interface
        /// </summary>
        Frontend,

        /// <summary>
        /// Changes to backend services or APIs
        /// </summary>
        Backend,

        /// <summary>
        /// Changes to infrastructure components like servers, databases, or networks
        /// </summary>
        Infrastructure,

        /// <summary>
        /// Changes to quality assurance processes or testing
        /// </summary>
        QA,

        /// <summary>
        /// Changes to data storage, retrieval, or processing
        /// </summary>
        Data,

        /// <summary>
        /// Changes to security mechanisms or policies
        /// </summary>
        Security,

        /// <summary>
        /// Changes to development operations or deployment processes
        /// </summary>
        DevOps,

        /// <summary>
        /// Changes to project documentation
        /// </summary>
        Documentation,

        /// <summary>
        /// Changes that span multiple specialization areas
        /// </summary>
        CrossFunctional,

        /// <summary>
        /// Changes that don't fit into other categories
        /// </summary>
        Other
    }
}