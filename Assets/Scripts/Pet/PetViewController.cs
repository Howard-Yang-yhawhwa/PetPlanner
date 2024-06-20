using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetViewController : MonoBehaviour
{
    [SerializeField] Transform viewContainer;
    [SerializeField] float growthAnimationPeriod = 1f;
     
    GameObject viewClone;
    GameObject viewPrefab;


    public GameObject InitializeDisplay(GameObject viewPrefab)
    {
        viewClone = Instantiate(viewPrefab, viewContainer);
        viewClone.transform.localPosition = viewPrefab.transform.position;
        this.viewPrefab = viewPrefab;

        return viewClone;
    }

    public void UpdateDisplay(PetData data, bool playAnim)
    {
        float targetScale = data.Level * 0.05f;
        Vector3 newScale = new Vector3(targetScale, targetScale, targetScale);

        EventBus.Publish(new SetCameraScaleEvent(targetScale));

        Debug.Log($"Scaling pet view ({data.Nickname}) to {newScale}");
        Debug.Log($"Position of pet view ({data.Nickname})'s prefab is {viewPrefab.transform.localPosition}");

        // Set the height of the pet view based on the pet's level
        viewClone.transform.localPosition = new Vector3(viewPrefab.transform.localPosition.x, viewPrefab.transform.localPosition.y * targetScale * 2, viewPrefab.transform.localPosition.z);

        // Scale the pet view based on the pet's level
        if (playAnim)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateDisplayRoutine(targetScale));
        }
        else
        {
            viewClone.transform.localScale = newScale;
        }
        
    }

    IEnumerator UpdateDisplayRoutine(float targetScale)
    {
        Vector3 newScale = new Vector3(targetScale, targetScale, targetScale);
        Vector3 startScale = viewClone.transform.localScale;
        
        float timer = 0;
        while (timer < growthAnimationPeriod) {
            timer += Time.deltaTime;
            viewClone.transform.localScale = Vector3.Lerp(startScale, newScale, timer / growthAnimationPeriod);
            yield return null;
        }
    }
}
