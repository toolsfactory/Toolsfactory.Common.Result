# Toolsfactory.Common Result and Result<T> utility library

The `Result` and `Result<T>` classes provide a robust way to represent the outcome of an operation in C#. This approach ensures operations can communicate success, failure, and relevant error details.

## Why use `Result` and `Result<T>`?
- **Clarity**: 
  - Clearly communicate the outcome of an operation without relying on exceptions.
  - Exceptions are for - as the name already implies - exceptional cases, not for regular control flow.
- **Error Handling**: 
  - Easily access error details when an operation fails.
- **Simplicity**: 
  - Simplify the handling of success and failure scenarios.
  - The code gets more readable and maintainable.
- **Performance**: 
  - Avoid the performance overhead of throwing and catching exceptions.

## Key Features

- **Encapsulation of Success/Failure**: 
  - The `Result` class encapsulates whether an operation was successful or not (`IsSuccess`).
  - Access to associated errors through the `Errors` collection.
  
- **Generic Support**: 
  - The `Result<T>` class extends `Result` to include a value (`T`) when the operation succeeds.
  
- **Immutable Design**:
  - Both classes are designed to ensure immutability, enhancing reliability and thread safety.

## Class Details

### `Result`
- Indicates whether an operation succeeded (`IsSuccess`) or failed (`IsFaulted`).
- Provides a collection of errors (`IReadOnlyList<Error>`) if the operation fails.
- Constructor overloading allows creating success results or faulted results with error details.

### `Result<T>`
- Extends `Result` by adding a value (`Value`) of type `T` for successful operations.
- Throws an exception if you try to access the `Value` of a failed operation.
- Ensures `Value` is cleared upon failure to prevent misuse.

### Extension methods
- `Switch` and `Map` methods are provided to simplify handling success and failure scenarios.
- `Bind`, `BindTryCatch`, and `Tap` methods are provided to simplify chaining operations following the Railway oriented pattern. (Inspired by this [Video](https://www.youtube.com/watch?v=C1oGnDEnS14))

# Usage Examples

### Creating a Success Result
```csharp
var successResult = Result.Success();
```

### Creating a Faulted Result
```csharp
var faultedResult = Result.Failure(new Error("Operation failed"));
```

### Using `Result<T>` with a Value
```csharp
var resultWithValue = Result<int>.Success(42);
if (resultWithValue.IsSuccess)
{
    Console.WriteLine(resultWithValue.Value); // Output: 42
}
```

### Using `Result<T>` with implicit conversion
```csharp
record Person(string Name, int Age);
var PersonDB = new List<Person> { new Person("Alice", 25), new Person("Bob", 30) };
var result1 = GetPersonWithName("Alice");
var result2 = GetPersonWithName("James");

Result<Person> GetPersonWithName(string Name)
{
    if (string.IsNullOrWhiteSpace(Name))
    {
        return new Error("Name cannot be empty"); // <= Implicit conversion to Result<Person>
    }
    var person = PersonDB.Where(p => p.Name == Name).FirstOrDefault();
    return person != null ? person : new Error("Person not found"); // <= Implicit conversion to Result<Person>
}


```

### Handling Errors
```csharp
if (result.IsFaulted)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error.Message);
    }
}
```

## Utility methods
Switch and Map methods are provided to simplify handling success and failure scenarios.
### Switch Method
```csharp
using System;
using System.Collections.Generic;
using Toolsfactory.Common;

class Program
{
    static void Main()
    {
        // Example 1: Success scenario
        var successResult = Result.Success();

        successResult.Switch(
            onSuccess: () => Console.WriteLine("Operation was successful!"),
            onFailure: errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error: {error.Message}");
                }
            }
        );

        // Example 2: Failure scenario
        var failureResult = Result.Failure(new List<Error>
        {
            new Error("Invalid input"),
            new Error("Connection timeout")
        });

        failureResult.Switch(
            onSuccess: () => Console.WriteLine("Operation was successful!"),
            onFailure: errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error: {error.Message}");
                }
            }
        );
    }
}
```

### Generic Switch Method
```csharp
using System;
using System.Collections.Generic;
using Toolsfactory.Common;

class Program
{
    static void Main()
    {
        // Example 1: Success scenario
        var successResult = Result<int>.Success(42);

        successResult.Switch(
            onSuccess: value => Console.WriteLine($"Success! Value: {value}"),
            onFailure: errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error: {error.Message}");
                }
            }
        );

        // Example 2: Failure scenario
        var failureResult = Result<int>.Failure(new List<Error>
        {
            new Error("Division by zero"),
            new Error("Value out of range")
        });

        failureResult.Switch(
            onSuccess: value => Console.WriteLine($"Success! Value: {value}"),
            onFailure: errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error: {error.Message}");
                }
            }
        );
    }
}
```


### Map Method
```csharp
using System;
using System.Collections.Generic;
using Toolsfactory.Common;
class Program
{
    static void Main()
    {
        // Example 1: Success scenario
        var successResult = Result.Success();

        string successMessage = successResult.Map(
            onSuccess: () => "Operation was successful!",
            onFailure: errors => string.Join(", ", errors.Select(e => e.Message))
        );

        Console.WriteLine(successMessage); // Output: Operation was successful!

        // Example 2: Failure scenario
        var failureResult = Result.Failure(new List<Error>
        {
            new Error("Invalid credentials"),
            new Error("Account locked")
        });

        string failureMessage = failureResult.Map(
            onSuccess: () => "Operation was successful!",
            onFailure: errors => string.Join("; ", errors.Select(e => e.Message))
        );

        Console.WriteLine(failureMessage); 
        // Output: Invalid credentials; Account locked
    }
}
```

### Generic Map Method
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Toolsfactory.Common;

class Program
{
    static void Main()
    {
        // Example 1: Success scenario
        var successResult = Result<int>.Success(100);

        string successMessage = successResult.Map(
            onSuccess: value => $"Operation succeeded with value: {value}",
            onFailure: errors => string.Join("; ", errors.Select(e => e.Message))
        );

        Console.WriteLine(successMessage); 
        // Output: Operation succeeded with value: 100

        // Example 2: Failure scenario
        var failureResult = Result<int>.Failure(new List<Error>
        {
            new Error("Network issue"),
            new Error("Timeout occurred")
        });

        string failureMessage = failureResult.Map(
            onSuccess: value => $"Operation succeeded with value: {value}",
            onFailure: errors => $"Errors: {string.Join(", ", errors.Select(e => e.Message))}"
        );

        Console.WriteLine(failureMessage);
        // Output: Errors: Network issue, Timeout occurred
    }
}
```

### Railway oriented pattern samples
planned for the next update
