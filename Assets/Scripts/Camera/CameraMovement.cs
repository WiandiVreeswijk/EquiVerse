using Cinemachine;
using UnityEngine;

namespace Input
{
    public class CameraMovement : MonoBehaviour
    {
        private bool movedLeft = false, movedRight = false, movedUp = false, movedDown = false;
        public bool CameraLocked { get; set; }
        private bool cameraStepCompleted = false;

        public CinemachineFreeLook mainCam;

        private void Start()
        {
            CameraLocked = true;
            mainCam.m_XAxis.m_InputAxisName = "";
            mainCam.m_YAxis.m_InputAxisName = "";
        }

        private void Update()
        {
            if (!CameraLocked && !cameraStepCompleted)
            {
                CheckCameraInput();
                CheckCameraTutorial();
            }
        }

        private void CheckCameraInput()
        {
            mainCam.m_XAxis.m_InputAxisName = "Horizontal";
            mainCam.m_YAxis.m_InputAxisName = "Vertical";
            
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                movedLeft = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                movedRight = true;
            }
            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                movedUp = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                movedDown = true;
            }
        }

        private void CheckCameraTutorial()
        {
            if (movedLeft && movedRight && movedUp && movedDown)
            {
                TutorialManager.CompleteStepAndContinueToNextStep("Step_Camera");
                cameraStepCompleted = true;
            }
        }
    }
}


