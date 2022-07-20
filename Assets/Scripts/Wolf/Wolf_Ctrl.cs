using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Ctrl : Enermy
{
    private Animator animator;
    private SpriteRenderer renderer;

    [System.Serializable]
    public enum WolfAni
    {
        Idle, Run, Attack, Die
    }

    [System.Serializable]
    public struct WolfStat
    {
        public int Wolf_Hp;
        public int Wolf_Power;
        public int Wolf_Speed;
        public int Wolf_Aumr;
    }

    [System.Serializable]
    public struct WolfNotChase
    {
        public int Vec;
        public double MoveMax;
        public bool isChase;
    }

    [Tooltip("´Á´ë¾Ö´Ï")]
    [SerializeField] private WolfAni wolfAni;
    private WolfState state;
    [Tooltip("´Á´ë½ºÅÈ")]
    [SerializeField] private WolfStat stat;
    [Tooltip("´Á´ëÀÎ½ÄÀü")]
    [SerializeField] private WolfNotChase NotChase;
    [Tooltip("Å¸ÄÏ:ÇÃ·¹ÀÌ¾î")]
    [SerializeField] private Transform Target;

    // Start is called before the fir
    // st frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(Wander());
        Move();
    }

    private void SetAni(WolfAni wolfAni)
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isRun", false);

        switch(wolfAni)
        {
            case WolfAni.Idle:
                break;
            case WolfAni.Run:
                break;
            case WolfAni.Attack:
                break;
            case WolfAni.Die:
                break;
        }
    }

    IEnumerator Attack()
    {
        

        yield return null;
    }

    protected override void Move()
    {
        if (NotChase.isChase)
        {
            SetAni(WolfAni.Run);
            if (transform.position.x > Target.position.x)
            {
                transform.Translate(Vector2.left * stat.Wolf_Speed * Time.deltaTime);
            }
            else if (transform.position.x < Target.position.x)
            {
                transform.Translate(Vector2.left * stat.Wolf_Speed * Time.deltaTime);
            }
        }
    }

    IEnumerator Wander()
    {
        if (!NotChase.isChase && NotChase.MoveMax <= 2)
        {
            SetAni(WolfAni.Run);
            if (NotChase.Vec > 0)
                renderer.flipX = true;
            else if (NotChase.Vec < 0)
                renderer.flipX = false;

            NotChase.MoveMax += Time.deltaTime;
            transform.Translate(Vector2.right * stat.Wolf_Speed * NotChase.Vec * Time.deltaTime);

            if (NotChase.MoveMax >= 2)
            {
                SetAni(WolfAni.Idle);
                yield return new WaitForSecondsRealtime(2);
                NotChase.Vec *= -1;
                NotChase.MoveMax = 0;
            }

        }

        yield return null;
    }
}
