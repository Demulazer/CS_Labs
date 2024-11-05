namespace TestLib;

// View = взаимодействие с пользователем
public class Menu : IMenu
{
    private readonly Dictionary<int, Func<Task>> _tasks;
    private readonly Dictionary<int, string> _tasksNames;

    public Menu(Dictionary<int, Func<Task>> tasks, Dictionary<int, string> tasksNames)
    {
        _tasks = tasks;
        _tasksNames = tasksNames;
    }

    public async Task DisplayHelp()
    {
        foreach (var key in _tasks.Keys)
            Console.WriteLine(key + " - " + _tasksNames[key]);
    }

    public async Task OptionsLoop()
    {
        do
        {
            await DisplayHelp();
            var readline = GetInput();
            await _tasks[await readline]();
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
            
            Console.WriteLine("Read key successfully");
            Console.Clear();
        } while (true);
    }

    private async Task<int> GetInput()
    {
        Console.WriteLine("Debug - we are in GetInput()");
        var userInput = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
        var validUserInput = false;
        foreach (var key in _tasks.Keys)
            if (key == userInput)
                validUserInput = true;

        if (!validUserInput)
        {
            Console.WriteLine("Please enter a valid number");
            return 1;
        }

        return userInput;
    }
}