// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Inference.Contexts.Parser
{
    /// <summary>
    /// Represents the data for a code window.
    /// This includes the starting line number and the size of the window.
    /// The window size must be at least 1.
    /// The default size is 25.
    /// The start line is 1-based.
    /// The window size is 0-based.
    /// The window size is the number of lines to show below the start line.
    /// </summary>
    internal record CodeWindowData
    {
        /// <summary>
        /// The center line number for the code window. Starts from 1.
        /// </summary>
        public int StartLine { get; set; } = 1;

        /// <summary>
        /// The number of lines to show below the start line. 
        /// Must be at least 1 and has a default of 100.
        /// </summary>
        public int WindowSize { get; set; } = 100;
    }

    /// <summary>
    /// Represents a view into a file's content, centered around a specific line.
    /// </summary>
    internal sealed record CodeWindow : CodeWindowData
    {
        /// <summary>
        /// Stashed code windows for later use.
        /// </summary>
        public required List<CodeWindowData> StashedWindows { get; init; }

        /// <summary>
        /// Content of the file being viewed.
        /// </summary>
        public required string FileContent { get; init; }

        /// <summary>
        /// Whether the code window is showing the last page of the full content
        /// </summary>
        public bool IsFinished => FileContent.Split(Environment.NewLine, StringSplitOptions.None).Length <= StartLine + WindowSize - 1;

        /// <summary>
        /// Gets the content of the code window, calculated based on FileContent, StartLine, and WindowSize.
        /// Includes line numbers. The window includes WindowSize lines below the StartLine.
        /// </summary>
        public List<CodeLine> WindowContent => CalculateWindowContent(FileContent, StartLine, WindowSize);

        /// <summary>
        /// The number of lines left in the file from the current start line.
        /// </summary>
        public int LinesLeft => FileContent.Split(Environment.NewLine, StringSplitOptions.None).Length - StartLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeWindow"/> record.
        /// Validates the WindowSize upon initialization.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if WindowSize is less than 1.</exception>
        public CodeWindow()
        {
            // Validate window size during initialization phase
            if ( WindowSize < 1 )
            {
                throw new ArgumentOutOfRangeException(nameof(WindowSize), "Window size must be at least 1.");
            }
        }

        /// <summary>
        /// Resizes the code window to a new size.
        /// Modifies the current instance.
        /// </summary>
        /// <param name="newWindowSize">The new size of the code window.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if newWindowSize is less than 1.</exception>
        public void ResizeWindow(int newWindowSize)
        {
            // Validate the new window size
            if ( newWindowSize < 1 )
            {
                throw new ArgumentOutOfRangeException(nameof(newWindowSize), "Window size must be at least 1.");
            }

            // Modify the WindowSize property of the current instance in-place.
            WindowSize = newWindowSize;
        }

        /// <summary>
        /// Advances the code window view by a specified number of lines in-place.
        /// Modifies the current instance.
        /// </summary>
        /// <param name="linesToScroll">The number of lines to advance the window start. Must be positive.</param>
        /// <returns>True if the window was scrolled successfully; false otherwise (e.g., invalid input or already at the end).</returns>
        public bool ScrollWindow(int linesToScroll)
        {
            // Validate input
            if ( linesToScroll <= 0 )
            {
                return false;
            }

            string[] fileLines = FileContent.Split(Environment.NewLine, StringSplitOptions.None);
            int totalLines = fileLines.Length;
            int originalStartLine = StartLine;

            // Calculate the new start line, clamping it within the valid range [1, totalLines].
            int newStartLine = Math.Clamp(StartLine + linesToScroll, 1, totalLines);

            // Check if scrolling actually changed the position
            if ( newStartLine == originalStartLine )
            {
                // Already at the end or beginning (if scrolling backwards was allowed)
                return false;
            }

            // Modify the StartLine property of the current instance in-place.
            StartLine = newStartLine;
            return true;
        }

        /// <summary>
        /// Advanced the code window view by a page with a specified number of overlapping lines.
        /// Modifies the current instance.
        /// </summary>
        /// <param name="overlapLines">The number of lines to overlap with the previous window.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if overlapLines is less than 0.</exception>
        public void PageWindow(int overlapLines)
        {
            // Validate the overlap lines
            if ( overlapLines < 0 )
            {
                throw new ArgumentOutOfRangeException(nameof(overlapLines), "Overlap lines must be at least 0.");
            }

            string[] fileLines = FileContent.Split(Environment.NewLine, StringSplitOptions.None);
            int totalLines = fileLines.Length;

            // Calculate the new start line, clamping it within the valid range [1, totalLines].
            int newStartLine = Math.Clamp(StartLine + WindowSize - overlapLines, 1, totalLines);

            // Modify the StartLine property of the current instance in-place.
            StartLine = newStartLine;
        }

        /// <summary>
        /// Resets the code window to the beginning of the file.
        /// Modifies the current instance.
        /// </summary>
        public void ResetWindowPosition() =>
            // Reset the StartLine to 1
            StartLine = 1;

        /// <summary>
        /// Calculates the content for a code window as a list of code lines.
        /// The window starts at the specified start line and includes the specified number of lines.
        /// </summary>
        /// <param name="fileContent">The full content of the file.</param>
        /// <param name="startLine">The starting line number (1-based) for the window.</param>
        /// <param name="windowSize">The number of lines to include in the window, starting from the start line.</param>
        /// <returns>A list of <see cref="CodeLine"/> objects representing the window content.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if windowSize is less than 10.</exception>
        private static List<CodeLine> CalculateWindowContent(string fileContent, int startLine, int windowSize)
        {
            // This check is technically redundant due to the constructor validation,
            // but good practice for a static helper method.
            if ( windowSize < 10 )
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be at least 10.");
            }

            if ( string.IsNullOrEmpty(fileContent) )
            {
                return [];
            }

            string[] fileLines = fileContent.Split([Environment.NewLine], StringSplitOptions.None);
            int totalLines = fileLines.Length;

            // Ensure startLine is within valid bounds (1 to totalLines)
            int clampedStartLine = Math.Clamp(startLine, 1, totalLines);

            // Calculate the window boundaries based on the start line and window size.
            // Note: The start line is 1-based, while array indices are 0-based.
            // We show 'windowSize' lines starting from 'clampedStartLine'.
            int windowStartIndex = Math.Clamp(clampedStartLine - 1, 0, totalLines - 1);
            int windowEndIndex = Math.Clamp(windowStartIndex + windowSize - 1, 0, totalLines - 1);

            // Ensure start index is not greater than end index (can happen if windowSize is large and startLine is near the end).
            if ( windowStartIndex > windowEndIndex )
            {
                // This case should only happen if totalLines is 0, which is handled by the IsNullOrEmpty check earlier.
                // Or if clampedStartLine is beyond the last line index after clamping, which shouldn't occur with Math.Clamp.
                // As a safeguard, return empty list if this unexpected state occurs.
                return [];
            }

            List<CodeLine> windowContentList = [];
            for ( int i = windowStartIndex; i <= windowEndIndex; i++ )
            {
                // Create a CodeLine object for the current line
                CodeLine codeLine = new()
                {
                    LineNumber = i + 1, // Line numbers are 1-based
                    Content = fileLines[i]
                };
                windowContentList.Add(codeLine);
            }

            return windowContentList;
        }

        /// <summary>
        /// Stashes the current code window for later use.
        /// </summary>
        public void StashWindow()
        {
            StashedWindows.Add(new CodeWindowData
            {
                StartLine = StartLine,
                WindowSize = WindowSize
            });
        }

        /// <summary>
        /// Pops the stashed code window, restoring it to the current context.
        /// </summary>
        public void PopWindow()
        {
            if ( StashedWindows.Count == 0 )
            {
                throw new InvalidOperationException("No stashed windows to pop.");
            }

            CodeWindowData lastStash = StashedWindows[^1];
            StashedWindows.RemoveAt(StashedWindows.Count - 1);

            StartLine = lastStash.StartLine;
            WindowSize = lastStash.WindowSize;
        }
    }

    /// <summary>
    /// Represents a line of code in the code window.
    /// </summary>
    internal sealed record CodeLine
    {
        /// <summary>
        /// The line number of the code line.
        /// </summary>
        public required int LineNumber { get; init; }

        /// <summary>
        /// The content of the code line.
        /// </summary>
        public required string Content { get; init; }
    }
}
