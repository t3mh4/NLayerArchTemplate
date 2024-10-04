namespace NLayerArchTemplate.CrudTemplate;

public static class Message
{
    public static void Info(string message, bool isNewLine = true)
    {
        if (isNewLine)
            Console.WriteLine(message);
        else
            Console.Write(message);
    }

    public static void Error(string message, bool isNewLine = true)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        if (isNewLine)
            Console.Error.WriteLine(message);
        else
            Console.Error.Write(message);
        Console.ResetColor();
    }
    public static void Success(string message, bool isNewLine = true)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        if (isNewLine)
            Console.Error.WriteLine(message);
        else
            Console.Error.Write(message);
        Console.ResetColor();
    }

    public static void Warning(string message, bool isNewLine = true)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        if (isNewLine)
            Console.Error.WriteLine(message);
        else
            Console.Error.Write(message);
        Console.ResetColor();
    }
}
