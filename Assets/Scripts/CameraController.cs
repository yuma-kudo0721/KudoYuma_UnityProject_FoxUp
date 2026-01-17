using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject cam;
    [SerializeField]
    GameObject player;

    void Start(){

    }

    void Update(){
        Vector3 a = cam.transform.position;
        cam.transform.position = new Vector3(a.x,player.transform.position.y,a.z);
        
    }
}
