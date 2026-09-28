namespace BoilerApplication.View;

public class BoilerDashboardView
{
    public void DisplayNotification()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("------------------Notifications-----------------");
        for(int i = 0; i < 10; i++)
        {
            Console.WriteLine(string.Empty);
        }
        Console.WriteLine("-------------------------------------------------");

    }

    public void DisplayUserMenu()
    {
        Console.SetCursorPosition(0, 15);
        Console.WriteLine("--------------------UserMenu---------------------");
        Console.WriteLine("[1] Start boiler sequence");
        Console.WriteLine("[2] Stop boiler sequence");
        Console.WriteLine("[3] Simulate Error");
        Console.WriteLine("[4] Toggle switch");
        Console.WriteLine("[5] Reset Lockout");
        Console.WriteLine("[6] View event logs");
        Console.WriteLine("[7] Exit application");
        Console.Write("Enter input option:");
    }

    public void DisplayEventLogs(string content)
    {
        Console.Clear();
        Console.WriteLine(content); 
    }
}
