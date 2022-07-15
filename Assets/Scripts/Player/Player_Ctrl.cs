using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Player_Ctrl : MonoBehaviour
{
    private SpriteRenderer renderer;
    protected Animator animator;
    private Rigidbody2D rb2D;
    private bool LastMove;

    [System.Serializable]
    public enum PlayerState
    {
        Idle, Run, Dash, Jump, ATK1, ATK2, JumpATK,
        SkillA, SkillS, SkillD
    }

    [System.Serializable]
    public struct PlayerStat
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
        [Tooltip("������Ÿ��")]
        public double ATK_Time;
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
    public PlayerStat PState;
    [Space(10f)]

    [Header("PlayerSkill")]
    [Tooltip("�÷��̾ų")]
    public PlayerTime PTime;
    [Space(10f)]

    [Header("PlayerCondition")]
    [Tooltip("�÷��̾����")]
    public PlayerCondition Cdn;
    [Space(10f)]

    [Header("PlayerState")]
    [Tooltip("�÷��̾�ִϸ��̼�����state")]
    [SerializeField] private PlayerState state;
    
    [SerializeField] private GameObject ATKObj;
    [SerializeField] private GameObject CharmGuk;
    [SerializeField] private GameObject NukBak;

    private void Awake()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Dash();
        Jump();
        Skill();
        StartCoroutine(Attack());
    }

    private void PlayerAni(string AniType)
    {
        state = (PlayerState)Enum.Parse(typeof(PlayerState), AniType);

        animator.SetBool("isRun", false);
        animator.SetBool("isDash", false);

        switch(state)
        {
            case PlayerState.Idle:
                animator.Play("Player_Idle");
                break;
            case PlayerState.Run:
                animator.SetBool("isRun", true);
                break;
            case PlayerState.Dash:
                animator.SetBool("isDash", true);
                animator.Play("Player_Dash");
                break;
            case PlayerState.Jump:
                animator.SetBool("isJump", true);
                break;
            case PlayerState.ATK1:
                animator.Play("Player_Attack1");
                break;
            case PlayerState.ATK2:
                animator.Play("Player_Attack2");
                break;
            case PlayerState.JumpATK:
                animator.Play("Player_JumpATK");
                break;
            case PlayerState.SkillA:
                animator.Play("Player_SkillA");
                break;
            case PlayerState.SkillS:
                animator.Play("Player_SkillS");
                break;
            case PlayerState.SkillD:
                animator.Play("Player_SkillD");
                break;

        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            PlayerAni("Run");
            //animator.SetBool("isRun", true);
            renderer.flipX = true;
            transform.Translate(Vector2.left * PState.PlayerSpeed * Time.deltaTime);
            LastMove = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            PlayerAni("Run");
            //animator.SetBool("isRun", true);
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
            PlayerAni("Dash");
            PTime.Dash_Time = 0;
            Cdn.Dashing = true;
            //animator.SetBool("isDash", true);
            //animator.SetBool("isJump", false);
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

        //animator.SetBool("isDash", false);
        Cdn.Dashing = false;
        PlayerAni("Idle");
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
        PTime.ATK_Time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C) && PTime.ATK_Time >= 1)
        {
            Vector2 Pos = new Vector2(0, 0);
            GameObject ATKct;
            PTime.ATK_Time = 0;

            if (Cdn.isGround)
            {
                switch (UnityEngine.Random.Range(1, 3))
                {
                    case 1:
                        PlayerAni("ATK1");
                        break;

                    case 2:
                        PlayerAni("ATK2");
                        break;
                }
                ATKObj.transform.localScale = new Vector3(3, 2, 1);

                if (LastMove == true)
                    Pos = new Vector2(transform.position.x - 1, transform.position.y - 0.2f);
                else if (LastMove == false)
                    Pos = new Vector2(transform.position.x + 1, transform.position.y - 0.2f);
            }
            else
            {
                Pos = new Vector2(transform.position.x, transform.position.y - 2);
                ATKObj.transform.localScale = new Vector3(3, 2, 1);
                PlayerAni("JumpATK");
            }

            ATKct = Instantiate(ATKObj, Pos, transform.rotation);
            Destroy(ATKct, 0.1f);
            yield return new WaitForSecondsRealtime(0.2f);

        }

        yield break;
    }

    private void Skill()
    {
        PTime.SkillA_Time += Time.deltaTime;
        PTime.SkillS_Time += Time.deltaTime;
        PTime.SkillD_Time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A) && PTime.SkillA_Time >= 7)
        {
            PlayerAni("SkillA");
            PTime.SkillA_Time = 0;
        }
        else if (Input.GetKeyDown(KeyCode.S) && PTime.SkillS_Time >= 14)
        {
            PlayerAni("SkillS");
            PTime.SkillS_Time = 0;
        }
        else if (Input.GetKeyDown(KeyCode.D) && PTime.SkillD_Time >= 30)
        {
            PlayerAni("SkillD");
            PTime.SkillD_Time = 0;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("isJump", false);
        PlayerAni("Idle");
        Cdn.isGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Cdn.isGround = false;
    }

    // �ִϸ��̼� �Լ�
    #region
    public void CreateCharmGuk()
    {
        var CharmGukObj = Instantiate(CharmGuk, transform.position, Quaternion.identity);
        Destroy(CharmGukObj, 1f);
    }

    public void CreateNukbak()
    {
        var NukBakObj = Instantiate(NukBak, transform.position, Quaternion.identity);
        Destroy(NukBakObj, 0.3f);
        NukBakObj.transform.DOScale(new Vector2(5, 5), 0.3f);
    }

    #endregion

}
