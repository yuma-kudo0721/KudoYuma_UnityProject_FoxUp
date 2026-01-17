using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScroll : MonoBehaviour
{
    [SerializeField]
    float scrollSpeed = 1f;
    [SerializeField]
    GameObject water;

    public Vector3 waterposition;

    public bool isStop = true;

    void Start()
    {
        waterposition = transform.position;

    }


    // Update is called once per frame
    void Update()
    {
        if (isStop) { return; } //isStopならスクロールしない
        Vector3 a = water.transform.position;
        a.y += scrollSpeed * Time.deltaTime;
        water.transform.position = a;

    }

    public void Reset()
    {
        water.transform.position = new Vector3(0, 0, 0);
    }
}
