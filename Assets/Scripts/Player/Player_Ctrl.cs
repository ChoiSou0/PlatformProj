using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ctrl : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Animator animator;
    private Rigidbody2D rb2D;

    [System.Serializable]
    public struct PlayerState
    {
        [Tooltip("플레이어체력")]
        public int PlayerHp;
        [Tooltip("플레이어방어력")]
        public int PlayerAmur;
        [Tooltip("플레이어스피드")]
        public int PlayerSpeed;
        [Tooltip("플레이어점프높이")]
        public int PlayerJumpPower;
        [Tooltip("플레이어공격력")]
        public int PlayerPower;
        [Tooltip("플레이어스킬A공격력")]
        public int SkillAPower;
        [Tooltip("플레이어스킬S공격력")]
        public int SkillSPower;
        [Tooltip("플레이어스킬D공격력")]
        public int SkillDPower;
    }

    [System.Serializable]
    public struct PlayerTime
    {
        [Tooltip("대쉬쿨타임")]
        public double Dash_Time;
        [Tooltip("플레이어스킬A쿨타임")]
        public double SkillA_Time;
        [Tooltip("플레이어스킬S쿨타임")]
        public double SkillS_Time;
        [Tooltip("플레이어스킬D쿨타임")]
        public double SkillD_Time;
    }

    [Header("PlayerState")]
    [Tooltip("플레이어스탯")]
    public PlayerState PState;
    [Space(10f)]

    [Header("PlayerSkill")]
    [Tooltip("플레이어스킬")]
    public PlayerTime PTime;

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
        Jump();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isRun", true);
            renderer.flipX = true;
            transform.Translate(Vector2.left * PState.PlayerSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isRun", true);
            renderer.flipX = false;
            transform.Translate(Vector2.right * PState.PlayerSpeed * Time.deltaTime);
        }
        else
            animator.SetBool("isRun", false);

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            rb2D.AddForce(Vector2.up * PState.PlayerJumpPower, ForceMode2D.Impulse);
        }
    }

    private void Attack()
    {

    }

}
