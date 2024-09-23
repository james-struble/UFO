using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offset;

    void Start () 
    {
        offset = gameObject.transform.position + player.transform.position;
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position + offset;
    }
}
