using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListController : MonoBehaviour
{
    [SerializeField] GameObject taskCreationUI;
    [SerializeField] Transform contentContainer;
    [SerializeField] TaskContent contentPrefab;
    [SerializeField] GameObject NoTaskHintUI;

    //Subscription<TodoListUpdatedEvent> todo_update_event;
    //Subscription<DoneListUpdatedEvent> done_update_event;
    Subscription<UpdateTaskList> update_display_event;

    Dictionary<string, TaskContent> contentDictionary = new Dictionary<string, TaskContent>();

    private void Awake()
    {
        //todo_update_event = EventBus.Subscribe<TodoListUpdatedEvent>(OnTodoListUpdate);
        //done_update_event = EventBus.Subscribe<DoneListUpdatedEvent>(OnDoneListUpdate);
        update_display_event = EventBus.Subscribe<UpdateTaskList>(OnUpdateDisplayEvent);
    }

    private void Start()
    {
        taskCreationUI.SetActive(false);
        UpdateDisplay();
    }

    public void OpenTaskCreationUI()
    {
        taskCreationUI.SetActive(true);
    }

    void OnUpdateDisplayEvent(UpdateTaskList e)
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // Populate Task List
        foreach (var kvp in TasksManager.TaskList)
        {
            string ID = kvp.Key;
            Task task = kvp.Value;

            if (!contentDictionary.ContainsKey(ID))
            {
                TaskContent clone = Instantiate(contentPrefab, contentContainer);
                clone.MyTaskID = ID;
                contentDictionary.Add(ID, clone);
            }

            contentDictionary[ID].UpdateDisplay();
            
        }

        // TODO: Add a cute animation to this
        NoTaskHintUI.SetActive(TasksManager.TaskList.Count <= 0);

    }

    /*
    void OnTodoListUpdate(TodoListUpdatedEvent e)
    {
        TaskContent clone = Instantiate(contentPrefab, contentContainer);
        string id = e.taskAddedID;
        clone.MyTaskID = id;
        todoContentDictionary.Add(id, clone);
        clone.transform.SetAsFirstSibling();
    }

    void OnDoneListUpdate(DoneListUpdatedEvent e)
    {
        if (todoContentDictionary.ContainsKey(e.taskRemovedID))
        {
            TaskContent targetedTaskContent = todoContentDictionary[e.taskRemovedID];
            todoContentDictionary.Remove(e.taskRemovedID);
            doneContentDictionary.Add(e.taskRemovedID, targetedTaskContent);
            targetedTaskContent.transform.SetAsLastSibling();
        }
    }
    */
}
