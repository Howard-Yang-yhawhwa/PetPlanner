using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetViewController : MonoBehaviour
{
    [SerializeField] Transform viewContainer;

    GameObject viewClone;

    public GameObject UpdateDisplay(GameObject viewPrefab)
    {
        viewClone = Instantiate(viewPrefab, viewContainer);
        viewClone.transform.localPosition = viewPrefab.transform.position;
        return viewClone;
    }
}
