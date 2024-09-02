using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ManuCtrl : MonoBehaviour
{
    [SerializeField] RectTransform pauseBG;
    [SerializeField] RectTransform pauseMenu;
    [SerializeField] RectTransform soundMenu;
    [SerializeField] RectTransform screenMenu;
    GameObject player;
    public bool isPause = false;

    void Start()
    {
        pauseBG = GameObject.Find("Canvas_UI").transform.GetChild(8).GetComponent<RectTransform>();
        pauseMenu = pauseBG.GetChild(0).GetComponent<RectTransform>();
        soundMenu = pauseBG.GetChild(1).GetComponent<RectTransform>();
        screenMenu = pauseBG.GetChild(2).GetComponent<RectTransform>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.U))
                Pause();

            SetCursor();
        }
    }

    void SetCursor()
    {
        var scripts = player.GetComponents<MonoBehaviour>();

        foreach(var script in scripts)
        {
            script.enabled = !isPause;
        }

        if(isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Pause()
    {
        isPause = !isPause;

        if (!pauseBG.gameObject.activeInHierarchy)
        {
            if (!pauseMenu.gameObject.activeInHierarchy)
            {
                pauseBG.gameObject.SetActive(isPause);
                pauseMenu.gameObject.SetActive(isPause);
                soundMenu.gameObject.SetActive(false);
                screenMenu.gameObject.SetActive(false);
            }
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        pauseBG.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        soundMenu.gameObject.SetActive(false);
        screenMenu.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    public void Sound(bool isOpen)
    {
        if (isOpen)
        {
            pauseMenu.gameObject.SetActive(false);
            soundMenu.gameObject.SetActive(true);
            screenMenu.gameObject.SetActive(false);
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            soundMenu.gameObject.SetActive(false);
        }
    }

    public void Screen(bool isOpen)
    {
        if (isOpen)
        {
            pauseMenu.gameObject.SetActive(false);
            soundMenu.gameObject.SetActive(false);
            screenMenu.gameObject.SetActive(true);
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            soundMenu.gameObject.SetActive(false);
            screenMenu.gameObject.SetActive(false);
        }
    }
}
