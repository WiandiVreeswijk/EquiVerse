using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = System.Drawing.Color;
using TMPro;

public class MainMenuBehaviour : MonoBehaviour
{
    [Header("SceneTransition")]
    public Image transitionOverlay;
    public GameObject loadingScreen;

    [Header("ToggleMenu's")] 
    public GameObject mainMenu;
    public GameObject optionsMenu;
    
    public void ClickPlay(int sceneIndex)
    {
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        })).SetUpdate(true);
    }

    public void ClickQuit()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        yield return operation;
        loadingScreen.SetActive(false);
    }

    public void ToggleOptionsMenu()
    {
        if (!optionsMenu.activeInHierarchy)
        {
            optionsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
