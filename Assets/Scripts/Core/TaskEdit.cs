using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskEdit : MonoBehaviour
{
    [SerializeField] GameObject EditUI;
    [SerializeField] GameObject EditInputs;
    [SerializeField] TMP_InputField NameInput;
    [SerializeField] TMP_InputField NotesInput;
    [SerializeField] TMP_Dropdown PriorityDropdown;
    [SerializeField] TMP_InputField EWTValueInput;
    [SerializeField] TMP_Dropdown EWTUnitsDropdown;

    [Space(15)]
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text NotesText;
    [SerializeField] TMP_Text PriorityText;
    [SerializeField] TMP_Text EWTText;
    [SerializeField] TMP_Text EditButtonText;

    [Space(30)]
    [Header("=== Debug Info ===")]
    [SerializeField] string inputName;
    [SerializeField] string inputNotes;
    [SerializeField] Priority inputPriority;
    [SerializeField] float inputEWTVal;
    [SerializeField] TimeUnits inputEWTUnits;

    bool isEditing = false;
    string currTaskID;

    Subscription<SetEditTaskEvent> set_task_event;

    private void Awake()
    {
        set_task_event = EventBus.Subscribe<SetEditTaskEvent>(SetTask);
    }

    private void Start()
    {
        inputName = "New Task";
        inputNotes = "";
        inputPriority = Priority.Backlog;
        inputEWTVal = 0;
        inputEWTUnits = 0;

        isEditing = false;
        EditInputs.SetActive(false);
    }

    void SetTask(SetEditTaskEvent e)
    {
        currTaskID = e.taskID;
        Task task = TasksManager.TaskList[currTaskID];

        Debug.Log($"Setting edit ui for {task.title}!");

        NameText.text = task.title;
        NotesText.text = task.notes;
        PriorityText.text = task.priority.ToString();
        EWTText.text = $"{task.etdValue}({task.etdUnits})";

        NameInput.text = task.title;
        NotesInput.text = task.notes;
        PriorityDropdown.value = (int)task.priority;
        EWTValueInput.text = task.etdValue.ToString();
        EWTUnitsDropdown.value = (int)task.etdUnits;

        EditUI.SetActive(true);
    }

    public void OnEditButtonPressed()
    {
        if (isEditing)
        {
            Dictionary<string, Task> tempList = TasksManager.TaskList;

            tempList[currTaskID].title = inputName;
            tempList[currTaskID].notes = inputNotes;
            tempList[currTaskID].priority = inputPriority;
            tempList[currTaskID].etdUnits = inputEWTUnits;
            tempList[currTaskID].etdValue = inputEWTVal;

            TasksManager.TaskList = tempList;

            EventBus.Publish(new SetEditTaskEvent(currTaskID));
        }

        isEditing = !isEditing;
        EditInputs.SetActive(isEditing);
        EditButtonText.text = isEditing ? "Done" : "Edit";
    }

    public void onNameInputChanged()
    {
        if (NameInput.text.Length <= 0) return;
        inputName = NameInput.text;
    }

    public void onNotesInputChanged()
    {
        if (NotesInput.text.Length <= 0) return;
        inputNotes = NotesInput.text;
    }

    public void onPriorityChanged()
    {
        inputPriority = (Priority)PriorityDropdown.value;
    }

    public void onEWTValChanged()
    {
        if (EWTValueInput.text.Length <= 0) return;
        inputEWTVal = float.Parse(EWTValueInput.text);
    }

    public void onEWTUnitsChanged()
    {
        inputEWTUnits = (TimeUnits)EWTUnitsDropdown.value;
    }
}
