using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sadasdas : MonoBehaviour
{
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        if (Target == null)
        Target = FindFirstObjectByType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Target.transform.position.y > transform.position.y)
        {
            transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }
}
