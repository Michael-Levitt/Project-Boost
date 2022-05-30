using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVec; // Range of movement in x,y,z
    [SerializeField] float period = 2f; // How fast one oscillation will be
    Vector3 startingPos;
    float movementFactor; // Position along movement range
    
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movementFactor = GenerateOscillatingFactor();
        Vector3 offset = movementVec * movementFactor;
        transform.position = startingPos + offset;
    }

    float GenerateOscillatingFactor()
    {
        if (period <= Mathf.Epsilon) { return 0f; } // Protect against 0 period
        float cycles = Time.time / period; // Counts number of cycles object has moved through
                                           // Continually grows

        const float tau = 2 * Mathf.PI;
        float rawSinWave = Mathf.Sin(cycles * tau); // magnitude [-1,1] in sine wave shape

        return movementFactor = (rawSinWave + 1) / 2f; // Oscillating sine wave in interval [0,1]
    }
}
