using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlantSpawner : MonoBehaviour
{
    private Camera mainCamera;

    public LayerMask groundLayer;
    public GameObject[] grassPrefabs;  // Prefab for the grass object

    [Header("GrassCost")]
    public float grassCost = 20f;

    public GameObject tutorialStep;

    public int maxPlants = 2;  // Maximum number of allowed plants
    private static int currentPlantCount = 0;  // Track the number of plants in the scene
    public GameObject maxPlantPopUp;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentPlantCount < maxPlants)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Raycast to detect the ground
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // Check if the hit point is on the NavMesh
                    if (IsPointOnNavMesh(hit.point) && ECManager.totalPoints >= grassCost)
                    {
                        int randomIndex = Random.Range(0, grassPrefabs.Length);
                        GameObject randomPrefab = grassPrefabs[randomIndex];
                        // Instantiate the grass prefab at the clicked position
                        GameObject spawnedPrefab = Instantiate(randomPrefab, hit.point, Quaternion.identity);
                        ECManager.totalPoints -= grassCost;
                        tutorialStep.SetActive(false);

                        currentPlantCount++; // Increment the plant count
                    }
                }
            }
            else
            {
                if (!maxPlantPopUp.activeInHierarchy)
                {
                    maxPlantPopUp.SetActive(true);
                }
            }
        }
    }

    // Check if a given point is on the NavMesh
    bool IsPointOnNavMesh(Vector3 point)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
    }

    // You should call this method when a plant is removed or destroyed.
    public static void RemovePlant()
    {
        if (currentPlantCount > 0)
        {
            currentPlantCount--;  // Decrement the plant count
        }
    }
}


