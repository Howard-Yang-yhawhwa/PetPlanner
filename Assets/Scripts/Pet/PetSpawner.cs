using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PetSpawner : MonoBehaviour
{
    [SerializeField] PetController PetPrefab;
    [SerializeField] Transform PetContainer;

    [Space(30)]
    [Header("Debug Info")]
    [SerializeField] bool DebugMode;
    [SerializeField] PetTypes TestSpawnType;

    public static Transform petContainer;
    public static PetController prefab;

    private void Awake()
    {
        prefab = PetPrefab;
        petContainer = PetContainer;
    }

    private void Start()
    {
        if (DebugMode)
        {
            SpawnNewPet(TestSpawnType);
        }

        foreach(var kvp in Player.OwnedPets)
        {
            string id = kvp.Key;
            SpawnPet(id);
        }

        // TODO: Remember what pet was last selected and display it here. 
        // For now, just choose the first pet.

        if (Player.OwnedPets.Count > 0)
        {
            string firstPetID = Player.OwnedPets.Keys.First();
            Debug.Log($"Setting {firstPetID} as selected in spawn pets");
            EventBus.Publish(new PetSelectedEvent(firstPetID));
        }
    }


    public static string SpawnNewPet(PetTypes type)
    {
        PetController newPet = Instantiate(prefab, petContainer);

        newPet.transform.position = petContainer.transform.position;

        PetData newData = PetManager.CreateData(type);

        Dictionary<string, PetData> ownedPet = Player.OwnedPets;
        ownedPet.Add(newData.ID, newData);
        Player.OwnedPets = ownedPet;

        newPet.Initialize(newData.ID);

        EventBus.Publish(new PetSpawnedEvent());
        EventBus.Publish(new PetSelectedEvent(newData.ID));

        return newData.ID;
    }


    public static void SpawnPet(string ID)
    {
        PetController newPet = Instantiate(prefab, petContainer);
        newPet.transform.position = petContainer.transform.position;
        newPet.Initialize(ID);

        EventBus.Publish(new PetSpawnedEvent());
    }
    
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
