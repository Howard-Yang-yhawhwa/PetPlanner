using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpawner : MonoBehaviour
{
    [SerializeField] PetController PetPrefab;
    [SerializeField] Collider ValidSpawnArea;

    [Space(30)]
    [Header("Debug Info")]
    [SerializeField] bool DebugMode;
    [SerializeField] PetTypes TestSpawnType;

    public static PetController prefab;
    public static Bounds validBounds;

    private void Awake()
    {
        prefab = PetPrefab;
    }

    private void Start()
    {
        if (DebugMode)
        {
            SpawnPet(TestSpawnType);
        }

        validBounds = ValidSpawnArea.bounds;

        foreach(var kvp in Player.OwnedPets)
        {
            string id = kvp.Key;
            SpawnPet(id);
        }
    }

    public static void SpawnPet(PetTypes type)
    {
        Vector3 spawnPos = RandomPointInBounds(validBounds);
        PetController newPet = Instantiate(prefab, spawnPos, Quaternion.identity);
        PetData newData = PetManager.CreateData(type);

        Dictionary<string, PetData> ownedPet = Player.OwnedPets;
        ownedPet.Add(newData.ID, newData);
        Player.OwnedPets = ownedPet;

        newPet.Initialize(newData.ID);
    }

    public void SpawnPet(string ID)
    {
        Vector3 spawnPos = RandomPointInBounds(validBounds);
        PetController newPet = Instantiate(prefab, spawnPos, Quaternion.identity);
        newPet.Initialize(ID);
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
