using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class Tutorial : MonoBehaviour
{
    private BoxCollider2D myCol;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private string TextToDisplay;
    [SerializeField] private TextMeshProUGUI uiTMP;


    // Start is called before the first frame update
    void Start()
    {
        myCol = GetComponent<BoxCollider2D>();
        myCol.size = boxSize;
        uiTMP = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterController>())
        {
            uiTMP.transform.parent.gameObject.SetActive(true);
            uiTMP.text = TextToDisplay;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterController>())
        {
            uiTMP.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, (Vector3)boxSize);
    }
}
