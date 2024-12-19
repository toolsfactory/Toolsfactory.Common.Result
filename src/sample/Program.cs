using Toolsfactory.Common;

public class Programm
{
    public static void Main()
    {
        SimpleDemo();
        ROPCallsDemo();
    }

    public static void SimpleDemo()
    {
        Console.WriteLine("Simple Demo");

        var result1 = Result<bool>.Success(true);

        Result<SampleData> result2 = Result<SampleData>.Success(new SampleData("John", 30));
        Result<SampleData> result3 = new SampleData("John", 30);
        Result<SampleData> result4 = new Error("Error message");
        var result5 = Result<SampleData>.Failure();
    }

    public static void ROPCallsDemo()
    {
        Console.WriteLine("ROP Calls Demo");

        Result<string> result = Result<bool>.Success(true)
                .Bind<bool, string>(x => x ? "Great!" : "Not so great...")
                .Tap(Console.WriteLine);
    }

    public record SampleData(string Name, int Age);
}