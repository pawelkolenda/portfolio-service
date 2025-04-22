namespace PortfolioService.Core.Models;

/// <summary>
/// Represents the result of an operation, including success status and an optional message.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Gets an optional message describing the result.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Creates a successful result with an optional message.
    /// </summary>
    /// <param name="message">The optional success message.</param>
    /// <returns>A successful <see cref="Result"/>.</returns>
    public static Result Ok(string? message = null)
    {
        return new Result { Success = true, Message = message };
    }

    /// <summary>
    /// Creates a failed result with a message.
    /// </summary>
    /// <param name="message">The failure message.</param>
    /// <returns>A failed <see cref="Result"/>.</returns>
    public static Result Fail(string message)
    {
        return new Result { Success = false, Message = message };
    }
}

/// <summary>
/// Represents the result of an operation with a data payload.
/// </summary>
/// <typeparam name="T">The type of the data payload.</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// Gets the data payload of the result.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Creates a successful result with data and an optional message.
    /// </summary>
    /// <param name="data">The data payload.</param>
    /// <param name="message">The optional success message.</param>
    /// <returns>A successful <see cref="Result{T}"/>.</returns>
    public static Result<T> Ok(T data, string? message = null)
    {
        return new Result<T> { Success = true, Data = data, Message = message };
    }

    /// <summary>
    /// Creates a successful empty result and an optional message.
    /// </summary>
    /// <param name="message">The optional success message.</param>
    /// <returns>A successful <see cref="Result{T}"/>.</returns>
    public static Result<T> Empty(string? message = null)
    {
        return new Result<T> { Success = true, Message = message };
    }

    /// <summary>
    /// Creates a failed result with a message.
    /// </summary>
    /// <param name="message">The failure message.</param>
    /// <returns>A failed <see cref="Result{T}"/>.</returns>
    public new static Result<T> Fail(string message)
    {
        return new Result<T> { Success = false, Message = message };
    }
}
