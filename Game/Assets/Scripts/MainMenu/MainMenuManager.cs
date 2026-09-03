using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Image transitioner;
    [SerializeField] private InputActionAsset Input;
    private InputActionMap map;
    private InputAction start;

    void OnEnable() { map.Enable(); }
    void OnDisable() { map.Disable(); }
    
    void Awake()
    {
        map = Input.FindActionMap("Game");
        start = map.FindAction("start");
    }

    void Start()
    {
        Time.timeScale = 1f;
        Set(true);
        transitioner.CrossFadeAlpha(1f, 0f, true);
        transitioner.CrossFadeAlpha(0f, 3f, false);
        StartCoroutine(SetAfterDelay(false, 3f));
    }

    void Update()
    {
        if(start.WasPressedThisFrame())
        {
            LoadGame();
        }
    }

    private void Set(bool status)
    {
        transitioner.gameObject.SetActive(status);
    }

    private IEnumerator SetAfterDelay(bool status, float delay)
    {
        yield return new WaitForSeconds(delay);
        Set(status);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
