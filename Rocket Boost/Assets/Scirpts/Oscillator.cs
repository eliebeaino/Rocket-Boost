using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;                      // speed of movement cycle

    float movementFactor;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)  return;   // protects against dividing by 0
        //set movement factor
        float cycles = Time.time / period;                      // grows continually from 0
        const float tau = Mathf.PI / 2f;                        // full circle circumference - ~6.28
        float rawSinWave = Mathf.Sin(cycles * tau);             // from -1 to 1
        movementFactor = rawSinWave / 2f + 0.5f;                // from 0 to 1

        // move the object
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
