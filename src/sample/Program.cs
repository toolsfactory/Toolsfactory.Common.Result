using System.Security.Cryptography.X509Certificates;
using Toolsfactory.Common;
Console.WriteLine("Hello, World!");

var result1 = Result<bool>.Success(true);

Result<SampleData> result2 = Result<SampleData>.Success(new SampleData("John", 30));
Result<SampleData> result3 = new SampleData("John", 30);
Result<SampleData> result4 = new Error("Error message");
var result5 = Result<SampleData>.Failure();

var y = result2.BindTryCatch<SampleData,SampleData,ArgumentException>(x => result3.Value,Error.Default);
public record SampleData(string Name, int Age);