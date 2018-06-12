using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

    public bool isLoading = false;
    FadeScript fs;
    public GameObject fade;

	// Use this for initialization
	void Start () {
        fs = fade.GetComponent<FadeScript>();
        fs.SetFadingIn(true, 0.0f, "");
	}
	
	// Update is called once per frame
	void Update () {
		if(isLoading && fs.isFinished) SceneManager.LoadScene("MainMenu");
    }

    private void OnMouseDown()
    {
        if (!isLoading)
        {
            fs.SetFadingIn(false, 0.0f, "");
            isLoading = true;
        }
    }
}
