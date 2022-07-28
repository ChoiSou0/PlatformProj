using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Ctrl : Enermy
{
    private Animator animator;
    private SpriteRenderer renderer;
    private Rigidbody2D rb;

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
        public int Wolf_BackPower;
        public int Wolf_Aumr;
        public int Wolf_ATKSpeed;
    }

    [System.Serializable]
    public struct WolfNotChase
    {
        public int Vec;
        public double MoveMax;
        public bool isChase;
        public bool isAttack;
        public double ATKTime;
        public double CoolTime;
        public float RGB;
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
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stat.Wolf_Hp <= 0)
            Die();
        else
        {
            StartCoroutine(Attack());
            StartCoroutine(Wander());
            Move();
        }

    }

    private void SetAni(WolfAni wolfAni)
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack", false);

        switch(wolfAni)
        {
            case WolfAni.Idle:
                animator.SetBool("isIdle", true);
                break;
            case WolfAni.Run:
                animator.SetBool("isRun", true);
                break;
            case WolfAni.Attack:
                animator.Play("Wolf_Attack");
                break;
            case WolfAni.Die:
                animator.Play("Wolf_Died");
                break;
        }
    }

    private void Die()
    {
        
        if (NotChase.RGB == 1)
            SetAni(WolfAni.Die);

        NotChase.RGB -= Time.deltaTime;
        renderer.color = new Color(1, 1, 1, NotChase.RGB);
        if (NotChase.RGB <= 0)
            Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        NotChase.CoolTime += Time.deltaTime;

        if (NotChase.isAttack && NotChase.CoolTime >= 3)
        {
            NotChase.isAttack = true;

            if (NotChase.ATKTime == 0)
                SetAni(WolfAni.Attack);

            NotChase.ATKTime += Time.deltaTime;

            if (NotChase.ATKTime <= 0.8f)
            {
                if (renderer.flipX)
                    transform.Translate(Vector2.right * stat.Wolf_ATKSpeed * Time.deltaTime);
                else
                    transform.Translate(Vector2.left * stat.Wolf_ATKSpeed * Time.deltaTime);
            }

            if (NotChase.ATKTime > 1)
            {
                SetAni(WolfAni.Run);
                NotChase.isAttack = false;
                NotChase.ATKTime = 0;
                NotChase.CoolTime = 0;
            }
        }

        yield return null;
    }

    protected override void Move()
    {
        Vector2 v = transform.position - Target.position;
        double r = Mathf.Abs(v.x) + Mathf.Abs(v.y);
        if (r < 8)
            NotChase.isChase = true;
        if (r < 5)
            NotChase.isAttack = true;


        if (NotChase.isChase)
        {
            SetAni(WolfAni.Run);
            if (transform.position.x > Target.position.x)
            {
                renderer.flipX = false;
                transform.Translate(Vector2.left * stat.Wolf_Speed * Time.deltaTime);
            }
            else if (transform.position.x < Target.position.x)
            {
                renderer.flipX = true;
                transform.Translate(Vector2.right * stat.Wolf_Speed * Time.deltaTime);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            switch(renderer.flipX)
            {
                case true:
                    rb.AddForce(Vector2.right * stat.Wolf_BackPower, ForceMode2D.Impulse);
                    break;
                case false:
                    rb.AddForce(Vector2.left * stat.Wolf_BackPower, ForceMode2D.Impulse);
                    break;
            }
            stat.Wolf_Hp -= PlayerStat.PlayerPower;
        }
    }

}
