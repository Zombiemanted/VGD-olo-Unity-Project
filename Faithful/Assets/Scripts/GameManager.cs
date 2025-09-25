using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;
public class GameManager : MonoBehaviour
{
    PlayerController player;
    Image healthbar;
    GameObject pauseMenu;

    public bool isPaused = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 1)
        {
            player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponent<PlayerController>();
            healthbar = GameObject.FindGameObjectWithTag("ui_health").GetComponent<Image>();

            pauseMenu = GameObject.FindGameObjectWithTag("Pause");
            pauseMenu.SetActive(false);
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 1)
            healthbar.fillAmount = (float) player.health / (float) player.maxHealth;
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseMenu.SetActive(true);

            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
            Resume();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadLevel(int level)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }
    public void MainMenu()
    {
        LoadLevel(0);
    }
    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);

            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
