public record ActionWindow
{
    /// <summary>
    /// The actions performed in order, latest is last.
    /// </summary>
    public required List<Action> Actions { get; set; }

    /// <summary>
    /// Currently visible window of actions.
    /// </summary>
    public List<Action>? Window => CalculateWindow();

    /// <summary>
    /// The number of actions to show in the window.
    /// Must be at least 1 and has a default of 25.
    /// </summary>
    public int WindowSize { get; set; } = 25;

    /// <summary>
    /// Calculates the current window of actions based on the specified window size.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private List<Action>? CalculateWindow()
    {
        // Validate the window size
        if (WindowSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(WindowSize), "Window size must be at least 1.");
        }
        // Ensure the Actions list is not null or empty
        if (Actions == null || Actions.Count == 0)
        {
            return null;
        }

        // Get latest actions based on the window size
        return Actions.TakeLast(WindowSize).ToList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionWindow"/> record.
    /// Validates the WindowSize upon initialization.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if WindowSize is less than 1.</exception>
    public ActionWindow()
    {
        if (WindowSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(WindowSize), "Window size must be at least 1.");
        }
    }

    /// <summary>
    /// Resizes the action window to a new size.
    /// Modifies the current instance.
    /// </summary>
    /// <param name="newWindowSize">The new size of the action window.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if newWindowSize is less than 1.</exception>
    public void ResizeWindow(int newWindowSize)
    {
        // Validate the new window size
        if (newWindowSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(newWindowSize), "Window size must be at least 1.");
        }

        // Modify the WindowSize property of the current instance in-place.
        WindowSize = newWindowSize;
    }

    /// <summary>
    /// Adds a new action to the action window.
    /// </summary>
    /// <param name="action">The action to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if action is null.</exception>
    /// <exception cref="ArgumentException">Thrown if action is not of type Action.</exception>
    public void AddAction(Action action)
    {
        // Validate the action
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), "Action cannot be null.");
        }

        // Ensure the Actions list is initialized
        Actions ??= [];

        // Add the action to the Actions list
        Actions.Add(action);
    }
}

public record Action
{
    /// <summary>
    /// Indicates whether the action was successful.
    /// </summary>
    public required bool IsSuccess { get; init; }

    /// <summary>
    /// The message associated with the action.
    /// If an error occurred, this will contain the error message.
    /// Can be null if successful.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// The item path associated with the action.
    /// </summary>
    public required ItemPath? ItemPath { get; init; }
}
