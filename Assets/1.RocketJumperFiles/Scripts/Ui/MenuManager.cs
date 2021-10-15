using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject controls;

    // Start is called before the first frame update
    void Start()
    {
        controls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        controls.SetActive(Input.GetKey(KeyCode.Tab));
    }

}
