using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // If instance is not yet assigned, try to find it in the scene
                _instance = FindObjectOfType<InputManager>();
                if (_instance == null)
                {
                    // If still null, create a new GameObject and attach the InputManager component
                    GameObject singletonObject = new GameObject("InputManager");
                    _instance = singletonObject.AddComponent<InputManager>();
                }
            }
            return _instance;
        }
    }

    public GameControls GameControls { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInput();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInput()
    {
        GameControls = new GameControls();
        EnablePlayerControls();
    }

    public void EnablePlayerControls()
    {
        GameControls.Player.Enable();
    }

    public void DisablePlayerControls()
    {
        GameControls.Player.Disable();
    }

    public void EnableUIControls()
    {
        // GameControls.UI.Enable();
    }

    public void DisableUIControls()
    {
        // GameControls.UI.Disable();
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}