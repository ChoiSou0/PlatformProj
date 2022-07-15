using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Ctrl : Enermy
{
    public int Speed;
    public int Vec;
    public double MoveMax;
    public bool isChase;


    // Start is called before the fir
    // st frame update
    void Start()
    {
        StartCoroutine(Wander());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Move()
    {
        
    }

    IEnumerator Wander()
    {
        if (!isChase && MoveMax <= 1)
        {
            MoveMax += Time.deltaTime;
            transform.Translate(Vector2.right * Speed * Vec * Time.deltaTime);

            if (MoveMax >= 1)
            {
                yield return new WaitForSecondsRealtime(1);
                Vec *= -1;
                MoveMax = 0;
            }

        }

        yield return null;
    }
}
