using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(MenuInputResolver))]
public class MenuInputManager : Singleton<MenuInputManager>
{

    [SerializeField] public InputActionAsset _inputActions;
    [SerializeField] public MenuInputResolver _menuInputResolver;
    [SerializeField] public GameManager gameManager;


    public InputAction pause;

    private Coroutine findGameManagerCoroutine;

    private void Reset()
    {
        _menuInputResolver = GetComponent<MenuInputResolver>();

        _inputActions = GetInputActionAsset();
    }

    protected override void Awake()
    {
        base.Awake();

        _inputActions.FindActionMap("Menu").Enable();
        SetActions();

        if (gameManager != null || findGameManagerCoroutine != null) return;
        StartCoroutine(FindGameManagerCoroutine());
    }

    private void Update()
    {
        _menuInputResolver.ResolveMenuInput(this, gameManager);
    }

    private void SetActions()
    {
        pause = _inputActions.FindAction("Pause");
    }

    private InputActionAsset GetInputActionAsset()
    {
        return Resources.Load("InputSystem_Actions") as InputActionAsset;
    }

    private IEnumerator FindGameManagerCoroutine()
    {
        GameManager gameManager = null;

        while(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            yield return null;
        }

        this.gameManager = gameManager;
    }
}
