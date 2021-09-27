using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CursorInformation
{
    public Sprite cursor;
    public Vector2 offset;

}

public class CursorController : MonoBehaviour
{

    [SerializeField] private Image cursorImg; 
    [SerializeField] private int currentCursor;
    [SerializeField] private List<CursorInformation> cursorInformations;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        cursorImg.sprite = cursorInformations[currentCursor].cursor;
    }

    // Update is called once per frame
    void Update()
    {
        cursorImg.GetComponent<RectTransform>().transform.position = Input.mousePosition + (Vector3)cursorInformations[currentCursor].offset;
    }

}
