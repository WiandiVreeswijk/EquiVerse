using UnityEngine;

namespace MainMenu
{
    public class LoadingIcon : MonoBehaviour
    {
        void Update() {
            transform.Rotate(new Vector3(0.0f, 100f * Time.unscaledDeltaTime, 0.0f));
        }
    }
}
