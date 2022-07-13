using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmGuk_Ctrl : MonoBehaviour
{
    public int CharmGuk_Speed;
    private SpriteRenderer Player_spriteRenderer;
    private SpriteRenderer spriteRenderer;
    private int Vec;

    private void Awake()
    {
        Player_spriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!Player_spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            Vec = 1;
        }
        else if (Player_spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            Vec = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * CharmGuk_Speed * Vec * Time.deltaTime);
    }
}
