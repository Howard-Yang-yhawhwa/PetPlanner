using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solstice.Core;

public enum Priority { VeryHigh = 4, High = 3, Medium = 2, Low = 1, Backlog = 0 }

[System.Serializable]
public class Task
{
    public string ID;
    public string title = "Default Task";
    public string notes = "---";
    public Priority priority = Priority.Backlog;
    public TimeUnits etdUnits = TimeUnits.Hour;
    public float etdValue = 1f;
    public bool isDone = false;
    public List<Subtask> Subtasks = new List<Subtask>();
}

[System.Serializable]
public class Subtask
{
    public string parentID;
    public string title = "New Subtask";
    public bool isDone = false;
}

public class TasksManager : MonoBehaviour
{
    [SerializeField] float SavePeriod = 60f;

    float lastSaved = 0;

    public static Dictionary<string, Task> TaskList { 
        get {
            return taskList; 
        } 
        set {
            taskList = value;
            EventBus.Publish(new UpdateTaskList());
        } 
    }

    public static Task TempTask;

    static Dictionary<string, Task> taskList = new Dictionary<string, Task>();

    private void Awake()
    {
        LoadData();
    }

    private void Update()
    {
        if (Time.time - lastSaved > SavePeriod)
        {
            SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    void LoadData()
    {
        taskList = SaveManager.Load<Dictionary<string, Task>>("Task_List");
        if (taskList == null)
        {
            taskList = new Dictionary<string, Task>();
            SaveManager.Save("Task_List", taskList);
        }
    }

    void SaveData()
    {
        SaveManager.Save("Task_List", taskList);
    }

    public static int CalcBounty(Task task)
    {
        int bounty = Mathf.RoundToInt((int)task.priority * GlobalConstants.BOUNTY_PIORITY_FACTOR * TimeUtils.ConvertTime(task.etdUnits, task.etdValue, TimeUnits.Second) * GlobalConstants.BOUNTY_ETD_FACTOR);
        Debug.Log($"Calculate bounty for ({task.title}) -- Task Priority = {(int)task.priority} * {GlobalConstants.BOUNTY_PIORITY_FACTOR} | Time = {TimeUtils.ConvertTime(task.etdUnits, task.etdValue, TimeUnits.Second)} * {GlobalConstants.BOUNTY_ETD_FACTOR} ---> Bounty: {bounty}");
        return bounty;
    }

    public static void AddTask(string ID, Task task)
    {
        Debug.Log($"Add Task Called! {task.title} - ({ID}) Added!");

        Dictionary<string, Task> tempList = TaskList;
        tempList.Add(ID, task);
        TaskList = tempList;

        PrintTaskList(TaskList, "TODO LIST");
    }

    public static void FinishedTask(string ID)
    {
        Debug.Log("Finished Task Called!");

        if (TaskList.ContainsKey(ID))
        {
            Player.Coins += CalcBounty(TaskList[ID]);

            Dictionary<string, Task> tempList = TaskList;
            tempList[ID].isDone = true;
            TaskList = tempList;
        }
    }

    public static string GetPrintTaskString(string ID, Task task)
    {
        string prntStr = $"{task.title} - ({ID}): \n" +
            $"\tDescription: {task.notes}\n" +
            $"\tDone: {task.isDone}\n" +
            $"===================================\n\n";
        return prntStr;
    }

    public static void PrintTaskList(Dictionary<string, Task> list, string name)
    {
        return;

        string str = $"Printing {name} ----->\n";
        foreach(var kvp in list)
        {
            str += GetPrintTaskString(kvp.Key, kvp.Value);
        }

        Debug.Log(str);
    }

}
