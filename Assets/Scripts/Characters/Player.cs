using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private float _attackTimer = 0f;
    private bool _canAttack = true;

    public Slider HealthSlider;
    public float Health;

    public float AttackRange;
    public float AttackDamage;
    public float AttackCooldown;

    public Transform SwordPosition;
    public LayerMask EnemyLayer;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    // Update is called once per frame
    void Update()
    {
        

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        UpdateHealth();

        TickAttackTimer();


        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            Run();
        } 
        else
        {
            Idle();
        }
            
    }

    void UpdateHealth()
    {
        HealthSlider.value = Health;
    }

    void Death()
    {
        m_animator.SetTrigger("Death");
    }

    public void Hurt(float damage)
    {
        m_animator.SetTrigger("Hurt");
        Health -= damage;
    }

    void Attack()
    {
        if (!_canAttack) return;
        m_animator.SetTrigger("Attack");
        Collider2D[] bandits = Physics2D.OverlapCircleAll(SwordPosition.position, AttackRange, EnemyLayer);
        for (int i = 0; i < bandits.Length; i++)
        {
            bandits[i].GetComponent<Bandit>().Hurt(AttackDamage);
        }
        ResetAttackTimer();
    }

    void ResetAttackTimer()
    {
        _attackTimer = AttackCooldown;
        _canAttack = false;
    }

    void TickAttackTimer()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer < 0) _canAttack = true;
    }

    //void Jump()
    //{
    //    if (m_grounded)
    //    {
    //        m_animator.SetTrigger("Jump");
    //        m_grounded = false;
    //        m_animator.SetBool("Grounded", m_grounded);
    //        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
    //        m_groundSensor.Disable(0.2f);
    //    }
    //}

    void Run()
    {
        m_animator.SetInteger("AnimState", 2);
    }

    void Idle()
    {
        if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);
        else
            m_animator.SetInteger("AnimState", 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(SwordPosition.position, AttackRange);
    }
}
