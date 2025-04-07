namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the quality assessment of a change
    /// </summary>
    public enum EnChangeQuality
    {
        /// <summary>
        /// Very Poor quality (unacceptable, severe issues)
        /// </summary>
        VeryPoor,

        /// <summary>
        /// Needs improvement (potential issues, incomplete, or not well-implemented)
        /// </summary>
        NeedsImprovement,

        /// <summary>
        /// Acceptable quality (meets minimum standards but could be better)
        /// </summary>
        Acceptable,

        /// <summary>
        /// Good quality (well-implemented and meets requirements)
        /// </summary>
        Good,

        /// <summary>
        /// Excellent quality (exceptional implementation, optimized, or innovative)
        /// </summary>
        Excellent,

        /// <summary>
        /// Outstanding quality (exceeds expectations, highly innovative, and sets a new standard)
        /// </summary>
        Outstanding
    }
}