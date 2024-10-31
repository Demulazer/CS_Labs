namespace TestLib;
// View = взаимодействие с пользователем
public class Menu : IMenu
{
    private Dictionary<int, Func<Task>> _tasks;
    private Dictionary<int, string> _tasksNames;

    public Menu(Dictionary<int, Func<Task>> tasks, Dictionary<int, string> tasksNames)
    {
        _tasks = tasks;
        _tasksNames = tasksNames;
        
    }
    public async Task DisplayHelp()
    {
        Console.WriteLine("Welcome");
        foreach (var key in _tasks.Keys)
        {
            Console.WriteLine(key + " - " + _tasksNames[key]);
        }
        await OptionsLoop();
    }

    public async Task OptionsLoop()
    {
        Console.WriteLine("Awaiting first input");
        var readline = GetInput();
        while(readline != -1)
        {
            await _tasks[readline]();
            Console.WriteLine("awaiting consecutive inputs");
            readline = GetInput();
        }
    }

    private int GetInput() 
    {
        var userInput = int.Parse(Console.ReadLine());
        bool validUserInput = false;
        foreach (var key in _tasks.Keys)
        {
            if(key == userInput) validUserInput = true;
        }

        if (!validUserInput)
        {
            Console.WriteLine("Please enter a valid number");
            return -1;
        }
        return userInput;
    }
}