using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskContent : MonoBehaviour
{
    [Header("=== References ===")]
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text bountyText;
    [SerializeField] Image backgroundImage;
    [SerializeField] Toggle toggle;
    [SerializeField] Button editButton;

    [Header("=== Settings ===")]
    [SerializeField] Color todoColor;
    [SerializeField] Color doneColor;

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
    bool initDoneVal;

    public void UpdateDisplay()
    {
        Task task = TasksManager.TaskList[ID];
        backgroundImage.color = task.isDone ? doneColor : todoColor;
        titleText.text = task.isDone ? "<s>" + task.title : task.title;
        bountyText.text = $"{TasksManager.CalcBounty(task)}";
        toggle.interactable = !task.isDone;
        initDoneVal = task.isDone;
        toggle.isOn = task.isDone;
        editButton.interactable = !task.isDone;
    }

    public void OnToggleValueChanged(bool status)
    {
        if (status && initDoneVal != status) TasksManager.FinishedTask(ID);
    }

    public void OnEditButton()
    {
        Debug.Log($"Edit Button pressed for {TasksManager.TaskList[MyTaskID].title}!");
        EventBus.Publish(new SetEditTaskEvent(MyTaskID));
    }

    public void DeleteTask()
    {
        if (!TasksManager.TaskList.ContainsKey(MyTaskID))
        {
            Debug.Log($"Error: Trying to delete a task ID ({MyTaskID}) that does not exist!");
            return;
        }

        TasksManager.TaskList.Remove(MyTaskID);
        EventBus.Publish(new UpdateTaskList());
        Destroy(gameObject);
    }
}
