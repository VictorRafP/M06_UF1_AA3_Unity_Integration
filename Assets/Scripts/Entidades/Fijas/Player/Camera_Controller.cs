using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public float alpha = 1.0f;

    public GameObject target = null;

    private Vector2 targetPosition;
    private Vector2 currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            targetPosition = target.transform.position;
            currentPosition = transform.position;

            transform.position = Vector2.Lerp(currentPosition, targetPosition, alpha * Time.deltaTime);
        }
    }
}
