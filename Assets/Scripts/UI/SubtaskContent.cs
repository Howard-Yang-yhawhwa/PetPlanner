using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtaskContent : MonoBehaviour
{
    [SerializeField] TMP_Text NameText;
    [SerializeField] Toggle CheckboxToggle;
    [SerializeField] Image BackImage;
    [SerializeField] Image ShadowImage;

    [Header("Done Visuals Configrations")]
    [SerializeField] Color BackColor;
    [SerializeField] Color ShadowColor;
    [SerializeField] Color TextColor;

    int myIndex;

    public void Setup(int index) {

        myIndex = index;

        Task currTask = TasksManager.TempTask;

        NameText.text = currTask.Subtasks[myIndex].title;

        if (currTask.Subtasks[myIndex].isDone)
        {
            ShowDoneVisual();
        }
    }

    public void ShowDoneVisual()
    {
        string taskName = NameText.text;
        NameText.text = $"<s>{taskName}</s>";
        CheckboxToggle.isOn = true;
        CheckboxToggle.interactable = false;

        BackImage.color = BackColor;
        ShadowImage.color = ShadowColor;
        NameText.color = TextColor;

        TasksManager.TempTask.Subtasks[myIndex].isDone = true;

        EventBus.Publish(new SubtasksUpdateEvent());
    }
}
