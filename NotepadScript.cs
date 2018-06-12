using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotepadScript : MonoBehaviour {

    SpriteInteract spriteScript;
    TextMesh tMesh;
    const float KEY_DELAY = 0.5f;
    float keyPressStartTime = -KEY_DELAY;
    const int MAX_LINE_NUMBER = 27;
    const int MAX_CHAR_PER_LINE = 30;
    bool isCursor = true;
    int charPerLine = 0;
    int lineNumber = 0;
    int[] charPerLineMemory = new int[MAX_LINE_NUMBER];

    // Use this for initialization
    void Start() {

        spriteScript = gameObject.GetComponentInParent<SpriteInteract>();
        tMesh = gameObject.GetComponent<TextMesh>();

    }

    // Update is called once per frame
    void Update() {
        if (spriteScript.isActive) UpdateText();

    }

    public void UpdateText()
    {
        string tempText = tMesh.text;
        //Debug.Log(charPerLine);
        string charCountString = Input.inputString;
        charCountString = charCountString.Replace("\b", "");
        charCountString = charCountString.Replace("\\n", "");
        charCountString = charCountString.Replace("\\", "");
        tempText = tempText.Replace("|", "");
        for (int i = 0; i < charCountString.Length; i++)
        {
            if (charPerLine >= MAX_CHAR_PER_LINE && lineNumber < MAX_LINE_NUMBER)
            {
                charPerLineMemory[lineNumber] = charPerLine;
                tempText += "\n";
                lineNumber++;
                charPerLine = 0;
            }
                
            else if (charPerLine >= MAX_CHAR_PER_LINE && lineNumber >= MAX_LINE_NUMBER)
            {
                return;
            }
            tempText += charCountString[i];
            charPerLine++;
                
        }
        

        if (Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Return))
        {
            if (Time.time - keyPressStartTime >= KEY_DELAY)
            {

                if (Input.GetKey(KeyCode.Backspace) && tempText.Length != 0)
                {
                    tempText = tempText.Replace("|", "");
                    string testStringForNewLine = tempText.Substring(tempText.Length - 1);
                    if (testStringForNewLine == "\n")
                    {
                        lineNumber--;
                        charPerLine = charPerLineMemory[lineNumber];
                    }
                    else charPerLine--;
                    tempText = tempText.Remove(tempText.Length - 1);
                    
                }
                if (Input.GetKey(KeyCode.Return) && lineNumber < MAX_LINE_NUMBER)
                {
                    charPerLineMemory[lineNumber] = charPerLine;
                    tempText = tempText + "\n";
                    lineNumber++;
                    charPerLine = 0;
                }
            }
            if (keyPressStartTime == -KEY_DELAY) keyPressStartTime = Time.time;
        }
        else keyPressStartTime = -KEY_DELAY;
        if (Mathf.Round((Time.time * 10f) % 5) == 0)
        {
            isCursor = !isCursor;
        }

        if (isCursor) tempText += "|";
        tMesh.text = tempText;
    }
}
