using System.Text.RegularExpressions;
using DG.Tweening;
using Managers;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Spawners {
    public class PlantSpawner : MonoBehaviour {
        private Camera mainCamera;

        [Header("Managers")]
        public GameManager gameManager;
        public ECManager ecManager;

        public LayerMask groundLayer;
        public GameObject grassPrefab;

        [Header("GrassCost")] public int grassCost = 20;

        [Header("Maximum plants")]
        public int maxPlants = 2; // Maximum number of allowed plants
        private static int currentPlantCount; // Track the number of plants in the scene
        public static bool canSpawnPlants = true;

        [Header("GuidedTutorialSetup")]
        public bool isTutorial;
        [ConditionalField("isTutorial")]
        public GameObject maxPlantPopUp;
        [ConditionalField("isTutorial")]
        public GameObject tutorialStep;

        private void Start() {
            mainCamera = Camera.main;
            currentPlantCount = 0;
        }

        public void ClickOnGround(Vector3 point) {
            if(!canSpawnPlants) return;
            if (IsPointOnNavMesh(point) && ECManager.totalPoints >= grassCost) {
                if (currentPlantCount < maxPlants) {
                    GameObject spawnedPrefab = Instantiate(grassPrefab, point, Quaternion.identity);
                    spawnedPrefab.transform.DOScale(1f, 0.75f).SetEase(Ease.OutElastic);
                    ecManager.DecrementPoints(grassCost);
                    tutorialStep.SetActive(false);

                    currentPlantCount++;
                    
                    FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/GrassPlacement");
                } else if (!maxPlantPopUp.activeInHierarchy && gameManager.tutorialActivated) {
                    maxPlantPopUp.SetActive(true);
                    PopInAnimation(maxPlantPopUp);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/PopupWarning");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                }
            }
        }

        bool IsPointOnNavMesh(Vector3 point) {
            NavMeshHit hit;
            return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
        }

        public static void RemovePlant() {
            if (currentPlantCount > 0) {
                currentPlantCount--;
            }
        }
        
        private void PopInAnimation(GameObject gameObject) {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            if (rectTransform != null) {
                rectTransform.localScale = new Vector3(0f, 0f, 0f);
                rectTransform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
            }
        }
    }
}
