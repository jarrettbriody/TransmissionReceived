using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public GameObject paperMenu;
    public GameObject fade;
    public FadeScript fs;
    bool clicked = false;
    DataLoader loader;
    public GameObject continueButton;

	// Use this for initialization
	void Start () {
        fs = fade.GetComponent<FadeScript>();
        fs.SetFadingIn(true, 0.0f, "");

        continueButton.SetActive(false);
        loader = new DataLoader();
        SaveFormat save = loader.GetSave();
        if (save != null)
        {
            if (save.day != 0 || save.level != 0)
            {
                continueButton.SetActive(true);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (fs.isFinished && clicked) SceneManager.LoadScene("GameScene");
	}

    public void ContinueOnClick()
    {
        if (!clicked)
        {
            clicked = true;
            PlayerPrefs.SetInt("loadState", 0);
            fs.SetFadingIn(false, 0.0f, "");
        }
    }

    public void NewGameOnClick()
    {
        if (!clicked)
        {
            clicked = true;
            PlayerPrefs.SetInt("loadState", 1);
            fs.SetFadingIn(false, 0.0f, "");
        }
    }

    public void OptionsOnClick()
    {
        paperMenu.SetActive(true);
    }

    public void ExitOnClick()
    {
        Application.Quit();
    }

    public void OptionsXOutOnClick()
    {
        paperMenu.SetActive(false);
    }
}
