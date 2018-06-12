using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour {

    public bool isFading;
    public bool fadingIn;
    public bool isFinished = false;
    int finishIndex = 0;
    bool isPingPonging = false;
    SpriteRenderer sr;
    Color cColor;
    float a;
    float timeout = 0.0f;
    float beginTime = 0.0f;
    string text = "";
    public GameObject textPrefab;
    GameObject textObject = null;
    TextMesh tMesh = null;
    int stringIndex = 0;
    float lastTime = 0;
    float randomTimeIncrement = 0;
    public AudioSource click;
    LevelManager levelManager;

	// Use this for initialization
	void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        GameObject obj = GameObject.Find("Game Manager");
        if (obj != null)
        {
            levelManager = obj.GetComponent<LevelManager>();
        }
        else
        {
            levelManager = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (isFading && !isFinished && Time.time >= beginTime + timeout && fadingIn) Fade();//
        else if (isFading && !isFinished && Time.time < beginTime + timeout && text != "" && fadingIn) DisplayText();
        if (isFinished && text != "") FadeText();
        if (isFinished && finishIndex == 1 && isPingPonging)
        {
            Debug.Log("switching fade");
            SetFadingIn(true, timeout, text);
        }
        */
        if (isFading && !isFinished)
        {
            if (!fadingIn)
            {
                Fade();
            }
            else
            {
                if (Time.time < beginTime + timeout && text != "") DisplayText();
                else
                {
                    if (Time.time >= beginTime + timeout) Fade();
                }
            }
        }
        if (isPingPonging && isFinished)
        {
            if (finishIndex == 1)
            {
                SetFadingIn(true, timeout, text);
                tMesh.color = new Color(tMesh.color.r, tMesh.color.g, tMesh.color.b, 1.0f);
            }
            if (finishIndex == 2)
            {
                isPingPonging = false;
                finishIndex = 0;
                isFinished = true;
                isFading = false;
            }
        }
        if (isFinished && text != "") FadeText();
    }

    void Fade()
    {
        cColor = sr.color;

        if (fadingIn)
        {
            a = cColor.a - 0.5f * Time.deltaTime;
            if (a < 0)
            {
                a = 0;
                isFinished = true;
                finishIndex++;
            }
            sr.color = new Color(cColor.r, cColor.g, cColor.b, a);
        }
        else
        {
            a = cColor.a + 0.5f * Time.deltaTime;
            if (a > 1)
            {
                a = 1;
                isFinished = true;
                finishIndex++;
            }
            sr.color = new Color(cColor.r, cColor.g, cColor.b, a);
        }
    }
    
    public void SetFadingIn(bool val, float tOut, string txt)
    {
        isFinished = false;
        fadingIn = val;
        timeout = tOut;
        text = txt;
        if (fadingIn)
        {
            cColor = sr.color;
            a = 1;
            sr.color = new Color(cColor.r, cColor.g, cColor.b, a);
        }
        else
        {
            cColor = sr.color;
            a = 0;
            sr.color = new Color(cColor.r, cColor.g, cColor.b, a);
        }
        isFading = true;
        beginTime = Time.time;
        
    }

    public void SetPingPong(float tOut, string txt)
    {
        finishIndex = 0;
        isPingPonging = true;
        SetFadingIn(false, tOut, txt);
    }

    void DisplayText()
    {
        if (textObject == null)
        {
            textObject = Instantiate(textPrefab);
            tMesh = textObject.GetComponent<TextMesh>();
            textObject.GetComponent<MeshRenderer>().sortingOrder = 11;
            lastTime = Time.time;
            randomTimeIncrement = Random.Range(1.0f, 2.0f);
        }
        if (Time.time > lastTime + randomTimeIncrement)
        {
            if (stringIndex < text.Length)
            {
                lastTime = Time.time;
                randomTimeIncrement = Random.Range(0.25f, 0.5f);
                tMesh.text += text[stringIndex];
                if (levelManager != null)
                {
                    click.volume = levelManager.soundVolume;
                    click.Play();
                }
                stringIndex++;
            }
        }
    }

    void FadeText()
    {
        a = tMesh.color.a - 0.5f * Time.deltaTime;
        if (a < 0)
        {
            a = 0;
            tMesh.text = "";
            stringIndex = 0;
        }
        tMesh.color = new Color(tMesh.color.r, tMesh.color.g, tMesh.color.b, a);
    }
}
