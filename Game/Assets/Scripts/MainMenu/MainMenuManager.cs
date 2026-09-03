using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Image transitioner;
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
