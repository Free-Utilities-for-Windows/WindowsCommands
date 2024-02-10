namespace WindowsCommands.Commands;

public static class Printer
{
    public static void StartPage()
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        string title = @"
__        ___           _                             
\ \      / (_)_ __   __| | _____      _____           
 \ \ /\ / /| | '_ \ / _` |/ _ \ \ /\ / / __|          
  \ V  V / | | | | | (_| | (_) \ V  V /\__ \          
  _\_/\_/  |_|_| |_|\__,_|\___/ \_/\_/ |___/    _     
 / ___|___  _ __ ___  _ __ ___   __ _ _ __   __| |___ 
| |   / _ \| '_ ` _ \| '_ ` _ \ / _` | '_ \ / _` / __|
| |__| (_) | | | | | | | | | | | (_| | | | | (_| \__ \
 \____\___/|_| |_| |_|_| |_| |_|\__,_|_| |_|\__,_|___/
                                                      
";
        Console.WriteLine(title);

        string text = "List all available commands:\n";
        Console.WriteLine(text);

        Console.ResetColor();
    }

    public static void AvailableCommands()
    {
        Console.ForegroundColor = ConsoleColor.White;
        
        string menu = "\n1 --> Activate command\n";
        menu += "0 --> Quit\n";
        Console.WriteLine(menu);
    }
}