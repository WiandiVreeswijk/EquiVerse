using DG.Tweening;
using Managers;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class TransitionOverlayToggle : MonoBehaviour
    {
        public Image transitionOverlay;
        public GameObject transitionOverlayObject;
        public GameObject guidedTutorial;
        public GameManager gameManager;
        
        [Header("isSecondTutorial")]
        public bool isSecondTutorial;
        [ConditionalField("isSecondTutorial")]
        public GameObject firstEnemyPopup;

        private void Start()
        {
            guidedTutorial.SetActive(false);
            transitionOverlayObject.SetActive(true);
            FadeSceneOpen();
        }

        private void FadeSceneOpen()
        {
            transitionOverlay.DOFade(0f, 4f).SetEase(Ease.InSine).OnComplete((() =>
            {
                if (gameManager.tutorialActivated)
                {
                    guidedTutorial.SetActive(true);
                    PopInAnimation(guidedTutorial);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                }

                if (gameManager.secondLevelTutorialActivated)
                {
                    firstEnemyPopup.SetActive(true);
                    firstEnemyPopup.transform.localScale = Vector3.zero;
                    PopInAnimation(firstEnemyPopup);
                }
            }));
        }

        private void FadeSceneClose()
        {
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic);
        }
        
        private void PopInAnimation(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform?.DOScale(1, 0.5f).SetEase(Ease.OutExpo).From(Vector3.zero);
        }
    }
}
