using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;


var TaskMenuOpen = false;
static void MainMenu() {
    Console.WriteLine("Welcome to the 2do-l1st!\n");
    Console.WriteLine("[1] Manage tasks");
    Console.WriteLine("[2] settings");

}

//this is menu navigation stuff

void DetectPress()
{
    var KeyPress = Console.ReadKey();
    if ( KeyPress.Key == ConsoleKey.D1)
    {
        TaskMenuOpen = true;
        TaskMenu();
    }

    else if (KeyPress.Key == ConsoleKey.D2)
    {
        Console.WriteLine("key2");
    } 
    else if (TaskMenuOpen == false )
    {
        Console.WriteLine("please press a valid key.");
    }
    else
    {
      //idk what 2 put here :P
    }
}

MainMenu();






while (true)
{
    DetectPress();   
}

//to show youre tasks, took me alotta debugging to get this one right :P

static TaskList Get()
{
    string basePath = AppDomain.CurrentDomain.BaseDirectory;
    string jsonpath = Path.Combine(basePath, "JSON", "taskconfig.json");

    string Djson = File.ReadAllText(jsonpath);

    var Dserialized = JsonConvert.DeserializeObject<TaskList>(Djson);


    return Dserialized;
   
}

 



void TaskMenu()
{
     

    int option = 1;
    string color = "\u001b[32m"; 
    string reset = "\u001b[0m";

    //also menu navigation
    while (true)
    {
        
   
        Console.Clear();
        Console.WriteLine("TASK LIST");
        Console.WriteLine("You are now viewing your tasks. Press [A] to add a task.");
        Console.WriteLine("Use arrow keys to select a task, then press [Enter] view and edit.");

        TaskList tasklist = Get();
        var taskarray = tasklist.Tasks.ToArray();

        foreach (var Tnumber in taskarray )
        {
            //messy string :P 
            Console.WriteLine(option == Tnumber.ID ? $"\n{color}> {Tnumber.Name}{reset}" : $"\n{Tnumber.Name}");
        }

        
        

        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.DownArrow:
                option++;
                break;

            case ConsoleKey.UpArrow:
                 option--;
                break;

            case ConsoleKey.Enter:
                
              
                break;

            case ConsoleKey.A:
                
              
                break;

            default:
                break;
        }
    }
}


static void SettingsMenu()
{
    Console.Clear();
    Console.WriteLine("placeholder #2");
}

//json class thingys
public class Task
{
    required public string Name;
    
    required public string Description;
    required public string Status;
    required public string CreatedAt;
    required public string UpdatedAt;
    required public int ID;

}

class TaskList
{
    required public List<Task> Tasks { get; set; }
}



