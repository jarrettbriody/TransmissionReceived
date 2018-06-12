using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffectScript : MonoBehaviour {

    Text cText;
    bool isOver = false;
    public AudioSource codeBeep;
    DataLoader loadData;
    SettingsFormat settings;

    // Use this for initialization
    void Start () {
        cText = GetComponent<Text>();
        loadData = new DataLoader();
        settings = loadData.GetSettings();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseOver()
    {
        //cText.color = new Color(140 / 255, 140 / 255, 140 / 255);
        if (!isOver)
        {
            settings = loadData.GetSettings();
            codeBeep.volume = settings.soundVolume;
            codeBeep.Play();
            cText.text = cText.text.Insert(0, ">");
            cText.text = cText.text.Insert(cText.text.Length, "<");
            isOver = true;
        }
    }

    private void OnMouseExit()
    {
        if (isOver)
        {
            cText.text = cText.text.Trim('>','<');
            cText.color = new Color(0f, 0f, 0f, 1f);
            isOver = false;
        }
    }

    private void OnMouseDown()
    {
        cText.color = new Color((197f / 255f), 0f, 0f,1f);
    }

    private void OnMouseUp()
    {
        cText.color = new Color(0f, 0f, 0f, 1f);
    }
}
