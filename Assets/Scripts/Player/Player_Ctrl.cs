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
        [Tooltip("�÷��̾�ü��")]
        public int PlayerHp;
        [Tooltip("�÷��̾����")]
        public int PlayerAmur;
        [Tooltip("�÷��̾�ǵ�")]
        public int PlayerSpeed;
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

    [Header("PlayerState")]
    [Tooltip("�÷��̾��")]
    public PlayerState PState;
    [Space(10f)]

    [Header("PlayerSkill")]
    [Tooltip("�÷��̾ų")]
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
