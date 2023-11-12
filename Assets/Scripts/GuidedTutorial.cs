using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GuidedTutorial : MonoBehaviour
{
    public GameObject screenOverlay;
    public GameObject guidedTutorial;
    public GameObject plantSpawner;
    public GameObject GameUI;
    public GameObject[] tutorialSteps;
    public GameObject objectSpawner;
    public GameObject rabbitButton;
    public GameObject finishLevelButton;
    public GameObject finishLevelStep;

    private int stepIndex = 0;
    private bool cameraStepCompleted = false;
    private bool finishLevelStepCompleted = false;
    private bool buttonPressed = false;

    private void Start()
    {
        screenOverlay.SetActive(true);
        guidedTutorial.SetActive(true);
        GameUI.SetActive(false);
        objectSpawner.SetActive(false);
        finishLevelButton.SetActive(false);
        plantSpawner.SetActive(false);
        
        PopInAnimation(guidedTutorial);
    }

    private void FixedUpdate()
    {
        if (CameraMovement.cameraMovedInAllDirections && !cameraStepCompleted)
        {
            GameUI.SetActive(true);
            tutorialSteps[stepIndex].SetActive(false);
            tutorialSteps[stepIndex + 1].SetActive(true);
            PopInAnimation(tutorialSteps[stepIndex + 1]);
            stepIndex += 1;
            cameraStepCompleted = true;
        }

        if (ECManager.totalPoints >= 150 && !finishLevelStepCompleted)
        {
            finishLevelButton.SetActive(true);
            finishLevelStep.SetActive(true);
            PopInAnimation(finishLevelStep);
            finishLevelStepCompleted = true;
        }
    }

    public void NextStep()
    {
        tutorialSteps[stepIndex].SetActive(false);
        tutorialSteps[stepIndex + 1].SetActive(true);
        PopInAnimation(tutorialSteps[stepIndex + 1]);
        stepIndex += 1;
        
        if (stepIndex == 2)
        {
            screenOverlay.SetActive(false);
            CameraMovement.cameraLocked = false;
        }

        if (stepIndex == 5)
        {
            rabbitButton.SetActive(true);
        }

        if (stepIndex == 7)
        {
            plantSpawner.SetActive(true);
        }
    }

    public void NextStepOnce()
    {
        if (!buttonPressed)
        {
            tutorialSteps[stepIndex].SetActive(false);
            tutorialSteps[stepIndex + 1].SetActive(true);
            PopInAnimation(tutorialSteps[stepIndex + 1]);
            stepIndex += 1;
            buttonPressed = true;
            objectSpawner.SetActive(true);
        }
    }

    public void FinishTutorial()
    {
        finishLevelStep.SetActive(false);
    }

    private void PopInAnimation(GameObject gameObject)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(0f, 0f, 0f);
            rectTransform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        }
    }
}
