using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCondition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        int theNumber = rand.Next(13, 101);
        Debug.Log(theNumber);

        if (theNumber > 13 && theNumber < 21)
        {
            Debug.Log("old person");
        }
        else if (theNumber >= 21)
        {
            Debug.Log("ancient");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
