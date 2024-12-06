# Toolsfactory.Common Result and Result<T> utility library

The `Result` and `Result<T>` classes provide a robust way to represent the outcome of an operation in C#. This approach ensures operations can communicate success, failure, and relevant error details without relying on exceptions.

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

## Usage Examples

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
Result<Person> GetPersonWithName(string Name)
{
    if (string.IsNullOrWhiteSpace(Name))
    {
        return Result.Failure<Person>(new Error("Name cannot be empty"));
    }
    var person = PersonDB.Where(p => p.Name == Name).FirstOrDefault(); // <= Assume PersonDB is a list of Person records
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

## Extensibility
Custom error handling or additional utilities can be added using the `ResultExtensions` class.

---

Let me know if you'd like further details added or the text tailored for a specific use case!