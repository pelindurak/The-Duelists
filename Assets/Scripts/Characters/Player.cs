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
    private float _attackTimer = 0f;
    private bool _canAttack = true;
    private bool _isDead = false;
    private float _inputX = 0f;

    public Slider HealthSlider;
    public float Health;

    public float AttackRange;
    public float AttackDamage;
    public float AttackCooldown;
    private float AttackAnimDelay = 0.5f;

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

        if (_isDead) return;

        // -- Handle input and movement --
        _inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (_inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (_inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_body2d.velocity = new Vector2(_inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);


        UpdateHealth();

        TickAttackTimer();


        if (Input.GetMouseButtonDown(0) && _canAttack)
        {
            Attack();
        }
        else if (Input.GetKeyDown("space") && m_grounded) 
        {
            Jump();
        }
        else if (Mathf.Abs(_inputX) > Mathf.Epsilon)
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
        if (Health <= 0) Death();
    }

    void Death()
    {
        m_animator.SetTrigger("Death");
        _isDead = true;
    }

    public void Hurt(float damage)
    {
        m_animator.SetTrigger("Hurt");
        Health -= damage;
    }

    void Attack()
    {
        if (!_canAttack) return;
        ResetAttackTimer();

        m_animator.SetTrigger("Attack");
        StartCoroutine(Damage());

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

    void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        m_groundSensor.Disable(0.2f);
    }

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

    IEnumerator Damage()
    {
        yield return new WaitForSecondsRealtime(AttackAnimDelay);
        Collider2D[] bandits = Physics2D.OverlapCircleAll(SwordPosition.position, AttackRange, EnemyLayer);
        for (int i = 0; i < bandits.Length; i++)
        {
            bandits[i].GetComponent<Bandit>().Hurt(AttackDamage);
        }
    }
}
