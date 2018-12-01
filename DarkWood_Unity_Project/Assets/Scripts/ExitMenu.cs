using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

    public GameObject exitMenuPanel;
    public GameObject gameOverPanel;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        if (exitMenuPanel == null)
        {
            Debug.Log("No exitMenuPanel ! Please add it in the appropriate field in the inspector, otherwise the game will lack an exit menu.");
        }
        exitMenuPanel.SetActive(false);

        gameOverPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitMenuPanel.SetActive(!exitMenuPanel.activeInHierarchy);
        }
	}

    //This function should be put as the action of the "exitMenuButton"
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
