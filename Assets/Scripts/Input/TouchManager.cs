using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEditor.PlayerSettings;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more then one dialogue manager in the scene");
        }
        Instance = this;
    }

    private void OnEnable()
    {
        //TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        EnhancedTouchSupport.Disable();
        //TouchSimulation.Disable();
    }

    private void FingerDown(Finger finger)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ScreenSaver")
        {
            if (ScreenSaverMain.Instance.isTouchable1 && !ScreenSaverMain.Instance.isTouchable2)
            {
                ScreenSaverMain.Instance.IsTouched();
                ScreenSaverMain.Instance.screenSaverText.enabled = true;
                return;
            }
            if (ScreenSaverMain.Instance.isTouchable1 && ScreenSaverMain.Instance.isTouchable2)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
                return;
            }
        }
        else
        {
            if (DialogueManager.instance && DialogueManager.instance.isDialoguePlaying == true)
            {
                DialogueManager.instance.ProceedDialogue();
                return;
            }
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
            var hit = Physics2D.OverlapPoint(touchPos);

            if (hit)
            {
                hit.transform.gameObject.SendMessage("Clicked", 0, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
