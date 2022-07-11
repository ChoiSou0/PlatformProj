using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Ctrl : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wander());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wander()
    {


        yield return null;
    }
}
