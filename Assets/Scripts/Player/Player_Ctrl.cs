using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ctrl : MonoBehaviour
{
    private SpriteRenderer renderer;
    protected Animator animator;
    private Rigidbody2D rb2D;
    private RaycastHit2D hit;

    private bool LastMove;

    [System.Serializable]
    public struct PlayerState
    {
        [Tooltip("�÷��̾�ü��")]
        public int PlayerHp;
        [Tooltip("�÷��̾����")]
        public int PlayerAmur;
        [Tooltip("�÷��̾�ǵ�")]
        public int PlayerSpeed;
        [Tooltip("�÷��̾�뽬���ǵ�")]
        public int PlayerDashSpeed;
        [Tooltip("�÷��̾���������")]
        public int PlayerJumpPower;
        [Tooltip("�÷��̾���ݷ�")]
        public int PlayerPower;
        [Tooltip("�÷��̾ųA���ݷ�")]
        public int SkillAPower;
        [Tooltip("�÷��̾ųS���ݷ�")]
        public int SkillSPower;
        [Tooltip("�÷��̾ųD���ݷ�")]
        public int SkillDPower;
    }

    [System.Serializable]
    public struct PlayerTime
    {
        [Tooltip("�뽬��Ÿ��")]
        public double Dash_Time;
        [Tooltip("�÷��̾ųA��Ÿ��")]
        public double SkillA_Time;
        [Tooltip("�÷��̾ųS��Ÿ��")]
        public double SkillS_Time;
        [Tooltip("�÷��̾ųD��Ÿ��")]
        public double SkillD_Time;
    }

    [System.Serializable]
    public struct PlayerCondition
    {
        [Tooltip("�뽬��")]
        public bool Dashing;
        [Tooltip("���� �ִ� ��")]
        public bool isGround;
    }

    [Header("PlayerState")]
    [Tooltip("�÷��̾��")]
    public PlayerState PState;
    [Space(10f)]

    [Header("PlayerSkill")]
    [Tooltip("�÷��̾ų")]
    public PlayerTime PTime;
    [Space(10f)]

    [Header("PlayerCondition")]
    [Tooltip("�÷��̾����")]
    public PlayerCondition Cdn;

    [SerializeField] private GameObject ATKObj;

    private void Awake()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Dash();
        Jump();
        StartCoroutine(Attack());
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isRun", true);
            renderer.flipX = true;
            transform.Translate(Vector2.left * PState.PlayerSpeed * Time.deltaTime);
            LastMove = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isRun", true);
            renderer.flipX = false;
            transform.Translate(Vector2.right * PState.PlayerSpeed * Time.deltaTime);
            LastMove = false;
        }
        else
            animator.SetBool("isRun", false);

    }

    private void Dash()
    {
        PTime.Dash_Time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X) && Cdn.Dashing == false && PTime.Dash_Time >= 2.5f)
        {
            PTime.Dash_Time = 0;
            Cdn.Dashing = true;
            animator.SetBool("isDash", true);
            animator.SetBool("isJump", false);
            switch (LastMove)
            {
                case true:
                    StartCoroutine(Dashed(-1));
                    break;

                case false:
                    StartCoroutine(Dashed(1));
                    break;
            }

        }

    }

    IEnumerator Dashed(int Vec)
    {
        for (int i = 0; i < 7; i++)
        {
            transform.Translate(Vector2.right * PState.PlayerDashSpeed * Vec * Time.deltaTime);
            yield return new WaitForSecondsRealtime(0.01f);
        }

        animator.SetBool("isDash", false);
        Cdn.Dashing = false;
        yield break;
    }


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Cdn.isGround == true)
        {
            rb2D.AddForce(Vector2.up * PState.PlayerJumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJump", true);
        }
    }

    IEnumerator Attack()
    {
        PTime.Dash_Time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C) && PTime.Dash_Time >= 1)
        {
            GameObject ATKct;
            PTime.Dash_Time = 0;

            switch (Random.Range(1, 3))
            {
                case 1:
                    animator.SetBool("isATK1", true);
                    break;

                case 2:
                    animator.SetBool("isATK2", true);
                    break;
            }


            if (LastMove == true)
            {
                Vector2 Pos = new Vector2(transform.position.x - 1, transform.position.y - 0.2f);
                yield return new WaitForSecondsRealtime(0.1f);
                ATKct = Instantiate(ATKObj, Pos, transform.rotation);
                Destroy(ATKct, 0.1f);
            }
            else if (LastMove == false)
            {
                Vector2 Pos = new Vector2(transform.position.x + 1, transform.position.y - 0.2f);
                yield return new WaitForSecondsRealtime(0.1f);
                ATKct = Instantiate(ATKObj, Pos, transform.rotation);
                Destroy(ATKct, 0.1f);
            }


            animator.SetBool("isATK1", false);
            animator.SetBool("isATK2", false);
        }

        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("isJump", false);
        Cdn.isGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Cdn.isGround = false;
    }

}
