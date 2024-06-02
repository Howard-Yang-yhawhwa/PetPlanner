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

    [Header("Debug Info")]
    [SerializeField] string inputName;
    [SerializeField] string inputNotes;
    [SerializeField] Priority inputPriority;
    [SerializeField] float inputEWTVal;
    [SerializeField] TimeUnits inputEWTUnits;

    private void Start()
    {
        inputName = "New Task";
        inputNotes = "";
        inputPriority = Priority.Backlog;
        inputEWTVal = 0;
        inputEWTUnits = 0;
    }

    public void onPrioritySelected(int value)
    {
        inputPriority = (Priority)value;
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

    public void onEWTValChanged()
    {
        if (EWTValueInput.text.Length <= 0) return;
        inputEWTVal = float.Parse(EWTValueInput.text);
    }

    public void onEWTUnitsChanged()
    {
        inputEWTUnits = (TimeUnits)EWTUnitsDropdown.value;
    }

    public void CreateTask()
    {
        string ID = RandomUtils.GenerateNumericCode(10);

        Task task = new Task();
        task.title = inputName;
        task.notes = inputNotes;
        task.priority = inputPriority;
        task.etdUnits = inputEWTUnits;
        task.etdValue = inputEWTVal;
        task.ID = ID;

        TasksManager.AddTask(task.ID, task);

        CreationUI.SetActive(false);
    }

    public void CloseCreationUI()
    {
        CreationUI.SetActive(false);
    }

}
