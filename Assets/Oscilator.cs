using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector = new Vector3(10f,10f,10f);
    [SerializeField] float Period = 2f;

    //TODO removefroim inspector
    [Range(0,1)]
    [SerializeField]
    float movementFactor; // 0 not move 4 full move
    Vector3 StartingPos;
    void Start()
    {
        StartingPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        if(Period<=Mathf.Epsilon)
        {
            return;

        }
        float cycles = Time.time / Period; //grows continually from 0
        const float tau = Mathf.PI * 2; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);


        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = MovementVector * movementFactor;
        transform.position = StartingPos + offset;
    }
}
