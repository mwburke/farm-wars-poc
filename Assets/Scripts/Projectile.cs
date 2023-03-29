using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float currentMoveTime;
    [SerializeField]
    private float moveTime;
    private GameObject target;
    private Vector3 startingPosition;
    [SerializeField]
    private float horizontalMoveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        currentMoveTime = 0f;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Just move it at constant speed until it reaches target transform position, then destroy
        // and trigger hit sound?
        // Is it fast enough we can just do it? 
    }
}
