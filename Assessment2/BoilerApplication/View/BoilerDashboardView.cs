namespace BoilerApplication.View;

public class BoilerDashboardView
{
    List<string> notifications = new List<string>();
    private object _lock = new object();
    public void DisplayNotification()
    {
        lock (_lock)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            
            for (int i = 0; i < 11; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
    }

    public void DisplayUserMenu()
    {
        lock(_lock)
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
    }

    public void ClearInputLine()
    {
        lock (_lock)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, 24);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
    }

    public void RefreshNotifications()
    {
        lock (_lock)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("------------------Notifications-----------------");
            for (int i = 0; i < notifications.Count; i++)
            {
                Console.WriteLine(notifications[i]);
            }


            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
        
    }

    public void AddMessage(string message)
    {
        notifications.Add(message);
        while(notifications.Count > 6)
        {
            notifications.RemoveAt(0);
        }

        DisplayNotification();
        RefreshNotifications();

    }

    public void DisplayEventLogs(string content)
    {
        lock( _lock)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, 26);
            Console.WriteLine(content);
            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
            Console.WriteLine(new string(' ', content.Length));
            Console.SetCursorPosition(cursorLeft, cursorTop);
            AddMessage("Viewed event logs");
            DisplayUserMenu();
        }   
    }  

    public void DisplayErrorMessage(string message)
    {
        lock (_lock)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, 26);
            Console.WriteLine(message);
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
        
    }
}
