using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskCreation : MonoBehaviour
{
    [SerializeField] GameObject CreationUI;
    [SerializeField] TMP_InputField NameInput;
    [SerializeField] TMP_InputField NotesInput;
    //[SerializeField] TMP_Dropdown PriorityDropdown;
    [SerializeField] TMP_InputField EWTValueInput;
    [SerializeField] TMP_Dropdown EWTUnitsDropdown;
    [SerializeField] Toggle[] PriorityToggles;

    [Header("=== Subtasks Stuff ===")]
    [SerializeField] TMP_Text SubtaskProgressText;
    [SerializeField] TMP_InputField SubtaskInput;
    [SerializeField] Transform SubtaskContainer;
    [SerializeField] SubtaskContent subtaskPrefab;

    [Header("Debug Info")]
    [SerializeField] Task CurrTempTask;

    bool isCreationMode;

    Subscription<SubtasksUpdateEvent> subtask_update_event;

    private void Awake()
    {
        subtask_update_event = EventBus.Subscribe<SubtasksUpdateEvent>(OnSubtaskUpdate);
    }

    private void Start()
    {
        TasksManager.TempTask = new Task();
    }

    private void Update()
    {
        CurrTempTask = TasksManager.TempTask;
    }

    public void onPrioritySelected(int value)
    {
        TasksManager.TempTask.priority = (Priority)value;
    }

    public void onNameInputChanged()
    {
        if (NameInput.text.Length <= 0) return;
        TasksManager.TempTask.title = NameInput.text;
    }

    public void onNotesInputChanged()
    {
        if (NotesInput.text.Length <= 0) return;
        TasksManager.TempTask.notes = NotesInput.text;
    }

    public void onEWTValChanged()
    {
        if (EWTValueInput.text.Length <= 0) return;
        TasksManager.TempTask.etdValue = float.Parse(EWTValueInput.text);
    }

    public void onEWTUnitsChanged()
    {
        TasksManager.TempTask.etdUnits = (TimeUnits)EWTUnitsDropdown.value;
    }

    public void OnSaveButtonClicked()
    {
        if (isCreationMode)
        {
            TasksManager.AddTask(TasksManager.TempTask.ID, TasksManager.TempTask);
        }
        else
        {
            TasksManager.TaskList[TasksManager.TempTask.ID] = TasksManager.TempTask;
        }
        
        CreationUI.SetActive(false);
    }

    public void CloseCreationUI()
    {
        CreationUI.SetActive(false);
    }

    public void CreateSubtask()
    {
        if (SubtaskInput.text.Length == 0)
        {
            return;
        }

        Subtask newSubtask = new Subtask();
        newSubtask.parentID = TasksManager.TempTask.ID;
        newSubtask.title = SubtaskInput.text;
        newSubtask.isDone = false;
        TasksManager.TempTask.Subtasks.Add(newSubtask);

        SubtaskContent subtaskClone = Instantiate(subtaskPrefab, SubtaskContainer);
        subtaskClone.Setup(TasksManager.TempTask.Subtasks.Count - 1);

        UpdateSubtaskProgressDisplay();

        SubtaskInput.text = "";
    }

    void UpdateSubtaskProgressDisplay()
    {
        int count = TasksManager.TempTask.Subtasks.Count;
        int doneCount = 0;

        foreach(Subtask task in TasksManager.TempTask.Subtasks)
        {
            if (task.isDone) doneCount++; 
        }

        string dispStr = count == 0 ? "No subtasks yet... Create one!" : $"Subtask Progress: {doneCount}/{count}";
        SubtaskProgressText.text = dispStr;
    }

    public void OpenCreationMode()
    {
        isCreationMode = true;
        string ID = RandomUtils.GenerateNumericCode(10);
        TasksManager.TempTask = new Task();
        TasksManager.TempTask.ID = ID;

        ClearUI();
        CreationUI.SetActive(true);
    }

    void ClearUI()
    {
        NameInput.text = "";
        NameInput.onValueChanged.Invoke("");


        NotesInput.text = "";
        NotesInput.onValueChanged.Invoke("");

        SubtaskInput.text = "";
        SubtaskInput.onValueChanged.Invoke("");


        foreach (Toggle toggle in PriorityToggles)
        {
            toggle.isOn = false;
        }

        PriorityToggles[0].isOn = true;

        EWTValueInput.text = "";
        EWTUnitsDropdown.value = 0;

        foreach(Transform child in SubtaskContainer)
        {
            Destroy(child.gameObject);
        }

        UpdateSubtaskProgressDisplay();
    }

    void OnSubtaskUpdate(SubtasksUpdateEvent e)
    {
        UpdateSubtaskProgressDisplay();
    }
}
