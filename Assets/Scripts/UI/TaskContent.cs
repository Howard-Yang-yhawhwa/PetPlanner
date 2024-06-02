using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskContent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text bountyText;
    [SerializeField] Image backgroundImage;
    [SerializeField] Toggle toggle;
    [SerializeField] Button editButton;

    [Header("Settings")]
    [SerializeField] Color todoColor;
    [SerializeField] Color doneColor;

    //Subscription<TodoListUpdatedEvent> todo_update_event;
    //Subscription<DoneListUpdatedEvent> done_update_event;

    public string MyTaskID
    {
        get
        {
            return ID;
        }

        set
        {
            ID = value;
            UpdateDisplay();
        }
    }

    string ID;

    private void Awake()
    {
        //todo_update_event = EventBus.Subscribe<TodoListUpdatedEvent>(OnTodoListUpdate);
        //done_update_event = EventBus.Subscribe<DoneListUpdatedEvent>(OnDoneListUpdate);
    }

    /*
    void OnTodoListUpdate(TodoListUpdatedEvent e)
    {
        
    }

    void OnDoneListUpdate(DoneListUpdatedEvent e)
    {
        if (e.taskRemovedID == ID)
        {
            UpdateDisplay();
        }
    }
    */

    public void UpdateDisplay()
    {
        Task task = TasksManager.TaskList[ID];
        backgroundImage.color = task.isDone ? doneColor : todoColor;
        titleText.text = task.isDone ? "<s>" + task.title : task.title;
        bountyText.text = $"{TasksManager.CalcBounty(task)}";
        toggle.interactable = !task.isDone;
        editButton.interactable = !task.isDone;
    }

    public void OnToggleValueChanged(bool status)
    {
        if (status) TasksManager.FinishedTask(ID);
    }

    public void OnEditButton()
    {
        Debug.Log($"Edit Button pressed for {TasksManager.TaskList[MyTaskID].title}!");
        EventBus.Publish(new SetEditTaskEvent(MyTaskID));
    }
}
