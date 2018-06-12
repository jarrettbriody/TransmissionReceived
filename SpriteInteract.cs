using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteInteract : MonoBehaviour {

    public bool isActive = false;
    Vector3 distanceFromMouse;
    LevelManager managerS;
    Vector3 screenMin;
    Vector3 screenMax;
    public GameObject desk;
    public GameObject dropoff;
    public GameObject deskArea;
    bool smallTransition;
    public bool isSmall;
    bool lastStateSmall;
    Vector3 lastPos;
    bool lerpR;
    float lerpPosR;
    float lerpStartTimeR;
    Vector3 lerpStartR, lerpEndR;
    public GameObject[] children;
    

    Vector3 lerpMaxSize, lerpMinSize;

    const float RESIZE_TIME = 0.15f;

    // Use this for initialization
    void Start () {
        
        managerS = GameObject.Find("Game Manager").GetComponent<LevelManager>();
        screenMin = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        desk = GameObject.Find("Desk");
        dropoff = GameObject.Find("Dropoff");
        deskArea = GameObject.Find("DeskArea");

        if (isSmall)
        {
            lerpMaxSize = gameObject.transform.localScale * 5;
            lerpMinSize = gameObject.transform.localScale;
        }
        else
        {
            lerpMaxSize = gameObject.transform.localScale;
            lerpMinSize = gameObject.transform.localScale / 5;
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckArrowInput();

        if (lerpR)
        {
            lerpPosR = ((Time.time - lerpStartTimeR) / RESIZE_TIME);

            if (lerpPosR >= 1)
            {
                lerpR = false;
                lerpPosR = 1;
            }

            gameObject.transform.localScale = Vector3.Lerp(lerpStartR, lerpEndR, lerpPosR);
        }

	}

    void setLerpSize(bool small)
    {
        if (lerpR)
        {
            Vector3 transfer = lerpStartR;
            lerpStartR = lerpEndR;
            lerpEndR = transfer;
        }
        else
        {
            if (small)
            {
                lerpStartR = lerpMaxSize;
                lerpEndR = lerpMinSize;
                lerpStartTimeR = Time.time;
                lerpR = true;
            }
            else
            {
                lerpStartR = lerpMinSize;
                lerpEndR = lerpMaxSize;
                lerpStartTimeR = Time.time;
                lerpR = true;
            }
        }
    }

    private void OnMouseDown()
    {
        isActive = true;
        managerS.telegraphActive = false;
        if(managerS.isActiveObject != gameObject)
        {
            Vector3 v;
            if (managerS.isActiveObject != null && managerS.isActiveObject != GameObject.Find("Telegraph"))
            {
                managerS.isActiveObject.GetComponent<SpriteInteract>().isActive = false;
                managerS.isActiveObject.GetComponent<SpriteRenderer>().sortingOrder -= 20;
                foreach (GameObject child in managerS.isActiveObject.GetComponent<SpriteInteract>().children)
                {
                    child.GetComponent<MeshRenderer>().sortingOrder -= 20;
                }
                v = managerS.isActiveObject.transform.position;
                v.z = 0;
                managerS.isActiveObject.transform.position = v;
                managerS.isActiveObject.transform.Find("highlight").gameObject.SetActive(false);
            }
            else if(managerS.isActiveObject == GameObject.Find("Telegraph"))
            {
                managerS.isActiveObject.GetComponent<TelegraphAnimate>().isActive = false;
                v = managerS.isActiveObject.transform.position;
                v.z = 0;
                managerS.isActiveObject.transform.position = v;
                managerS.isActiveObject.transform.Find("highlight").gameObject.SetActive(false);
            }
            
            v = gameObject.transform.position;
            v.z = -5;
            gameObject.transform.position = v;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder += 20;
            foreach (GameObject child in children)
            {
                child.GetComponent<MeshRenderer>().sortingOrder += 20;
            }
            managerS.isActiveObject = gameObject;
            gameObject.transform.Find("highlight").gameObject.SetActive(true);
        }
        distanceFromMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
    }

    private void OnMouseDrag()
    {
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - distanceFromMouse;

        if (!smallTransition)
        {
            if (!isSmall)
            {
                CheckBounds(desk.GetComponent<SpriteRenderer>().bounds.min, desk.GetComponent<SpriteRenderer>().bounds.max);
            }
            else
            {
                CheckBounds(dropoff.GetComponent<SpriteRenderer>().bounds.min, dropoff.GetComponent<SpriteRenderer>().bounds.max);
            }
        }
        else
        {
            if (testSmallChange())
            {
                if (!isSmall)
                {
                    setLerpSize(true);
                    isSmall = true;
                    
                    distanceFromMouse = Vector3.zero;
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, gameObject.transform.position.z);
                }
            }
            else
            {
                if (isSmall)
                {
                    setLerpSize(false);
                    isSmall = false;
                }
            }

            CheckBounds(deskArea.GetComponent<SpriteRenderer>().bounds.min, deskArea.GetComponent<SpriteRenderer>().bounds.max);
        }
        ZeroZPos();
    }

    bool testSmallChange()
    {
        bool small = false;

        if (gameObject.transform.position.y < desk.transform.position.y - desk.GetComponent<SpriteRenderer>().bounds.extents.y)
        {
            small = true;
        }

        if (gameObject.transform.position.x > desk.transform.position.x + desk.GetComponent<SpriteRenderer>().bounds.extents.x)
        {
            small = true;
        }

        return small;
    }

    private void OnMouseUp()
    {
        if (smallTransition)
        {
            if (CheckWithin(desk.GetComponent<SpriteRenderer>().bounds.min, desk.GetComponent<SpriteRenderer>().bounds.max))
            {
                if (isSmall)
                {
                    isSmall = false;
                    gameObject.transform.localScale *= 5;
                }
                smallTransition = false;
                CheckBounds(desk.GetComponent<SpriteRenderer>().bounds.min, desk.GetComponent<SpriteRenderer>().bounds.max);
            }
            else if (CheckWithin(dropoff.GetComponent<SpriteRenderer>().bounds.min, dropoff.GetComponent<SpriteRenderer>().bounds.max))
            {
                smallTransition = false;
                isSmall = true;
                CheckBounds(dropoff.GetComponent<SpriteRenderer>().bounds.min, dropoff.GetComponent<SpriteRenderer>().bounds.max);
            }
            else
            {
                if (!lastStateSmall)
                {
                    if (isSmall)
                    {
                        gameObject.transform.localScale = lerpMaxSize;
                        lerpR = false;
                        isSmall = false;
                    }
                    smallTransition = false;

                    gameObject.transform.position = lastPos;
                }
                else
                {
                    smallTransition = false;
                    gameObject.transform.position = lastPos;
                }
            }
        }
    }

    bool CheckWithin(Vector3 min, Vector3 max)
    {
        bool hit = false;
        Vector3 pos = gameObject.transform.position;
        if ((pos.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x >= min.x) &&
        (pos.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x <= max.x) &&
        (pos.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y >= min.y) &&
        (pos.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y <= max.y))
        {
            hit = true;
        }

        return hit;
    }

    void ZeroZPos()
    {
        Vector3 tempVec = gameObject.transform.position;
        if (!isActive) tempVec.z = 0;
        else tempVec.z = -5;
        gameObject.transform.position = tempVec;
    }

    void CheckBounds(Vector3 min, Vector3 max)
    {
        bool transition = false;
        Vector3 pos = gameObject.transform.position;
        if (pos.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x < min.x)
        {
            pos.x = min.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
        }
        if (pos.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x > max.x)
        {
            transition = true;
            pos.x = max.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
        }
        if (pos.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y < min.y)
        {
            if (!isSmall) { transition = true; }
            pos.y = min.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y;
        }
        if (pos.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y > max.y)
        {
            if (isSmall) { transition = true; }
            pos.y = max.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y;
        }

        gameObject.transform.position = pos;

        if (transition && !smallTransition)
        {
            smallTransition = true;
            if (isSmall)
            {
                lastStateSmall = true;
            }
            else
            {
                lastStateSmall = false;
            }
            lastPos = gameObject.transform.position;
        }
        
    }

    void CheckArrowInput()
    {
        if (isActive)
        {
            
            Vector3 eulerAng = gameObject.transform.rotation.eulerAngles;
            eulerAng.z += Input.GetAxis("Mouse ScrollWheel") * 50;
            gameObject.transform.rotation = Quaternion.Euler(eulerAng);
        }
    }
}
