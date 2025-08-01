using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;


//I MADE THIS SHIT ON A WINDOWS VM ON LINUZX, YOU HEARD THAT.. THGE LUNIX! AND THE VM SETUP TOOK ME FUCKING FOREVER PLEASE HELP 

var TaskMenuOpen = false;
TaskList tasklist = Get();


void MainMenu() {
    Console.SetWindowSize(84, 20);
    Console.SetBufferSize(84, 20);
    Console.Write(@"

                                                                                 
                                                                                 
      ,----,                                 ,--,     ,---,              ___     
    .'   .' \     ,---,                    ,--.'|  ,`--.' |            ,--.'|_   
  ,----,'    |  ,---.'|   ,---.      ,---,.|  | : /    /  :            |  | :,'  
  |    :  .  ;  |   | :  '   ,'\   ,'  .' |:  : ':    |.' '  .--.--.   :  : ' :  
  ;    |.'  /   |   | | /   /   |,---.'   ,|  ' |`----':  | /  /    '.;__,'  /   
  `----'/  ;  ,--.__| |.   ; ,. :|   |    |'  | |   '   ' ;|  :  /`./|  |   |    
    /  ;  /  /   ,'   |'   | |: ::   :  .' |  | :   |   | ||  :  ;_  :__,'| :    
   ;  /  /-,.   '  /  |'   | .; ::   |.'   '  : |__ '   : ; \  \    `. '  : |__  
  /  /  /.`|'   ; |:  ||   :    |`---'     |  | '.'||   | '  `----.   \|  | '.'| 
./__;      :|   | '/  ' \   \  /           ;  :    ;'   : | /  /`--'  /;  :    ; 
|   :    .' |   :    :|  `----'            |  ,   / ;   |.''--'.     / |  ,   /  
;   | .'     \   \  /                       ---`-'  '---'    `--'---'   ---`-'   
`---'         `----'                                                             
                                                                                 

");
    Console.WriteLine("[1] Manage tasks");
    Console.WriteLine("[2] Credits & misc.");
  

    while (true)
    {
        DetectPress();
    }

}

//this is menu navigation stuff

void DetectPress()
{
    var KeyPress = Console.ReadKey();
    if ( KeyPress.Key == ConsoleKey.D1)
    {
       
        TaskMenu();
    }

    else if (KeyPress.Key == ConsoleKey.D2)
    {
       SettingsMenu();  
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









void Add()
{
    string color = "\u001b[32m";
    string reset = "\u001b[0m";
    Console.Clear();
    Console.WriteLine("Welcome to the Add Task Menu!");
    Console.WriteLine("If you want to go back, close this program and reopen it.\n");

    string name, description, status;

    
    Console.Write("Please type in the name for your task: ");
    name = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Task name cant be empty");
        Console.Write("Please type in the name for your task: ");
        name = Console.ReadLine();
    }
    Console.WriteLine("The name of this task is: " + name);

    
    Console.Write("\nPlease type a description for your task: ");
    description = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(description))
    {
        Console.WriteLine("description cant be empty.");
        Console.Write("Please type a description for your task: ");
        description = Console.ReadLine();
    }
    Console.WriteLine("The description of this task is: " + description);


    Console.Write("\nPlease make a status for your task (it can be anything): ");
    status = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(status))
    {
        Console.WriteLine("status cant be empty");
        Console.Write("Please make a status for your task (it can be anything): ");
        status = Console.ReadLine();
    }
    Console.WriteLine("The status for this task is: " + status);

    var newtask = new TaskNM
    {
        Name = name,
        Description = description,
        Status = status,
        CreatedAt = DateTime.Now.ToString(),
        UpdatedAt = DateTime.Now.ToString(),
        ID = tasklist.Tasks.Count + 1
    };

    tasklist.Tasks.Add(newtask);
    Save(tasklist);
  

    Console.WriteLine("\nYippee! You've made a task!");
    Console.WriteLine("(Press [B] to go back.)");



    while (true)
    {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.B)
        {
            break;
        }

    }
}







void Edit(int IDedit)
{
    TaskMenuOpen = false;
    Console.Clear();

    var taskedit = tasklist.Tasks.FirstOrDefault(t => t.ID == IDedit);

    if (taskedit != null)
    {
        Console.WriteLine($"Editing Task: {taskedit.Name}\n");
    }
    else
    {
        Console.Clear();
        Console.WriteLine("Task not found.");
        Thread.Sleep(1000);
        TaskMenu();
    }
        Console.WriteLine("press [N] to edit the name,");
    Console.WriteLine("press [D] to edit the description");
    Console.WriteLine("and press [S] to edit the status\n\n");

    Console.WriteLine("press [R] to REMOVE this task.");
    Console.WriteLine("press [B] to go back.");

    Console.WriteLine("Press [ENTER] 2 times to confirm.");



    var keypress = Console.ReadKey(true);
    switch (keypress.Key)
    {

        case ConsoleKey.N:
            Console.WriteLine("\nEnter a new name");
            taskedit.Name = Console.ReadLine();
            
            break;

        case ConsoleKey.D:
            Console.WriteLine("\nEnter a new description");
            taskedit.Description = Console.ReadLine();
            break;
        case ConsoleKey.S:
            Console.WriteLine("\nEnter new status");
            taskedit.Status = Console.ReadLine();
            break;

        case ConsoleKey.B:
            Console.Clear();
            TaskMenu();
            break;

        case ConsoleKey.R:
            tasklist.Tasks.Remove(taskedit);
            IDchange();
            Save(tasklist);
            TaskMenu();
              //i struggled hella on this bruh
            break;
    }
    taskedit.UpdatedAt = DateTime.Now.ToString();
    Save(tasklist);
   

  


}
  

//to show youre tasks, took me alotta debugging to get this one right :P
TaskList Get()
{
    string workingDirectory = AppContext.BaseDirectory;
    string basePath = Directory.GetParent(workingDirectory).FullName;
    
    string jsonpath = Path.Combine(basePath, "JSON", "taskconfig.json");
    
    string Djson = File.ReadAllText(jsonpath);

    var Dserialized = JsonConvert.DeserializeObject<TaskList>(Djson);





    return Dserialized;


   
}



void IDchange()
{
    for (int i = 0; i < tasklist.Tasks.Count; i++)
    {
        tasklist.Tasks[i].ID = i + 1;
    }
}

void Save(TaskList taskList)
{
    string workingDirectory = AppContext.BaseDirectory;
    string basePath = Directory.GetParent(workingDirectory).FullName;
    string jsonpath = Path.Combine(basePath, "JSON", "taskconfig.json");

    string updatedJson = JsonConvert.SerializeObject(taskList, Formatting.Indented);
    File.WriteAllText(jsonpath, updatedJson);
}


void TaskMenu()
{
     

    int option = 1;
  TaskMenuOpen = true;
    

    //also menu navigation



    feach();

void feach()
    {
        Console.Clear();
        Console.WriteLine("TASK LIST");
        Console.WriteLine("you are now viewing your tasks. press [A] to add a task.");
        Console.WriteLine("use [UP] and [DOWN] to select a task, then press [ENTER] to edit or remove.");
        Console.WriteLine("press [V] to view the task.");
        Console.WriteLine("press [B] to go back.");



        foreach (var Tnumber in tasklist.Tasks)
        {
            
            //messy string :O
            Console.WriteLine(option == Tnumber.ID ? $"\n--> {Tnumber.Name} (Status: {Tnumber.Status})" : $"\n{Tnumber.Name} (Status: {Tnumber.Status})");
            
        }
        
       
    }







    while (true)
        {
            var key = Console.ReadKey(true);
           
                switch (key.Key)
                {

                case ConsoleKey.DownArrow:
                    option++;
                    
                    feach();
                    break;

                case ConsoleKey.UpArrow:
                    option--;
                    
                    feach();
                    break;


                case ConsoleKey.Enter:

                    Edit(option);

                    break;

                case ConsoleKey.V:

                    view(option);

                    break;

                case ConsoleKey.A:

                        Add();
                        break;

                    case ConsoleKey.B:
                        Console.Clear();
                        MainMenu();
                        break;

                    default:
                        break;

                }
            



        }
    
        
   

}

void view(int id)
{
    TaskMenuOpen = false;
    Console.Clear();
    var taskview = tasklist.Tasks.FirstOrDefault(t => t.ID == id);
    if (taskview == null)
    {
        Console.WriteLine("Task not found.");
        Thread.Sleep(1500);
        TaskMenu();
        return;
    }

    Console.WriteLine("Main\n");
    Console.WriteLine($"Name: {taskview.Name}");
    Console.WriteLine($"Description: {taskview.Description}");
    Console.WriteLine($"Status: {taskview.Status}");

    Console.WriteLine("\nmisc.\n");
    Console.WriteLine($"Created at: {taskview.CreatedAt}");
    Console.WriteLine($"Updated at: {taskview.UpdatedAt}");
    Console.WriteLine($"Task ID: {taskview.ID}");

    Console.WriteLine("\nPress [B] to go back.");

    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.B)
    {
        TaskMenu();
    }
}

void SettingsMenu()
{


    Console.Clear();
    Console.WriteLine("Hello!\n");
    Console.WriteLine("If you have any issues, please refer to my github repo: https://github.com/Litdude101/2do-l1st");
    Console.WriteLine("This was made by Litdude101 on github");
    Console.WriteLine("\nThis is my first c# project, i learned alot, and yeah, so long, my fellow humans!");
    Console.WriteLine("\nASCII art was made with patorjk taag.");
    Console.WriteLine("\n(Press B to go back.)");
    while (true)
    {
        TaskMenuOpen = true;
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.B:
                Console.Clear();
                MainMenu();
                
                break;

            default:
                break;
        }
    }

}





//json class thingys
public class TaskNM
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
    required public List<TaskNM> Tasks { get; set; }
}



