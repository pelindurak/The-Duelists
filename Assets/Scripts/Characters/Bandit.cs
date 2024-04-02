using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Bandit : MonoBehaviour {

    public float m_speed = 4.0f;
    //[SerializeField] float      m_jumpForce = 7.5f;

    public Animator m_animator { get; set; }
    public Rigidbody2D m_body2d { get; set; }
    public Sensor_Bandit m_groundSensor { get; set; }
    public bool m_grounded { get; set; } = false;

    private FuzzyMain fuzzyScript;

    public Slider BanditHealthSlider, PlayerHealthSlider;
    public float _attackTimer { get; set; } = 0f;
    public bool _canAttack { get; set; } = true;
    public bool _isDead { get; set; } = false;
    public bool _isPlayerDead { get; set; } = false;

    public float BanditHealth;
    public float PlayerHealth;

    public float AttackRange;
    public float AttackDamage;
    public float AttackCooldown;
    public float AttackAnimDelay = 0.5f;

    public bool IsFsmActive, IsFuzzyLogicActive;

    public bool IsSimulation = false;

    public Transform SwordPosition;
    public LayerMask PlayerLayer;
    public GameObject PlayerObject;

    public TMP_Text aggressionText;

    public Transform LeftCollider, RightCollider;


    Stack<BaseState> stateStack = new Stack<BaseState>();
    public float HealthDiff = 0f;

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();

        fuzzyScript = GetComponent<FuzzyMain>();
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (_isDead || _isPlayerDead) return;

        UpdateHealth();

        TickAttackTimer();

        DecideAction();

    }

    public void DecideAction()
    {
        if (IsFuzzyLogicActive)
        {
            EvaluateAggression();
            return;
        }
        else if (IsFsmActive)
        {
            RunFsm();
        }
        else
        {
            ChillIdle();
            aggressionText.text = "";
        }
    }

    public void EnterFsm()
    {
        stateStack.Push(new IdleState(this));
    }

    public void RunFsm()
    {
        DecideFsmAction();
        RunAction();
    }

    public void RunAction()
    {
        BaseState currentState = stateStack.Peek();
        currentState.PreSwap();
        currentState.RunState();
        currentState.PostSwap();
        aggressionText.text = currentState.GetName();
    }

    public void DecideFsmAction()
    {
        if (stateStack.Count == 0 || PlayerHealth <= 0)
        {
            stateStack.Push(new IdleState(this));
        }

        else if (BanditHealth <= 0)
        {
            if (stateStack.Peek() is not DeathState)
            {
                stateStack.Push(new DeathState(this));
            }
            return;
        }
        
        switch (stateStack.Peek())
        {
            case IdleState:
                if (PlayerHealth <= 0)
                {
                    return;
                }
                else if (BanditHealth >= PlayerHealth)
                {
                    stateStack.Push(new ChasePlayerState(this));
                }
                else
                {
                    stateStack.Push(new StandGroundState(this));
                }

                break;
            case AttackState:
                if (!IsCloseToPlayer() || BanditHealth < PlayerHealth)
                {
                    stateStack.Pop();
                }

                break;
            case ChasePlayerState:
                if (IsCloseToPlayer())
                {
                    stateStack.Push(new AttackState(this));
                }
                else if (BanditHealth < PlayerHealth)
                {
                    stateStack.Push(new StandGroundState(this));
                }

                break;
            case DeathState:

                break;
            case RunAwayState:
                if (ReachedMapEdge())
                {
                    stateStack.Pop();
                }

                break;
            case StandGroundState:
                if (BanditHealth >= PlayerHealth)
                {
                    stateStack.Pop();
                }
                else if ((BanditHealth < PlayerHealth - HealthDiff) && !ReachedMapEdge())
                {
                    stateStack.Push(new RunAwayState(this));
                }

                break;
            default:
                stateStack.Push(new IdleState(this));
                break;
        }
    }

    public void EvaluateAggression()
    {
        float aggro = fuzzyScript.GetCrispAggro();

        if (aggro >= 40)
        {
            aggressionText.text = "Offence";
            Offence();
        }
        else if (aggro >= 30)
        {
            aggressionText.text = "StandGround";
            Idle();
            StandGround();
        }
        else
        {
            aggressionText.text = "Defense";
            Defense();
        }
    }

    // follow player
    // attack as much as you can
    public void Offence()
    {
        if (IsCloseToPlayer()) Attack();
        else ChasePlayer();
    }

    // follow player
    // attack & retreat
    public void StandGround()
    {
        SwapSpriteDirection(PlayerObject.transform.position.x - transform.position.x);
        if (IsCloseToPlayer()) Attack();
    }

    // retreat
    public void Defense()
    {
        // if the bandit reaches the edges of the play area,
        // he turns around and faces the player
        if (ReachedMapEdge())
        {
            Idle();
            StandGround();
        }
        else
        {
            RunAway();
        }

    }

    public void ChasePlayer()
    {
        float inputX = 0f;
        if (!IsCloseToPlayer()) 
        {
            float xDiff = PlayerObject.transform.position.x - transform.position.x;
            inputX = Mathf.Clamp(xDiff, -1f, 1f);
            Run(inputX);
        }
    }

    public void RunAway()
    {
        float xDiff = PlayerObject.transform.position.x - transform.position.x;
        float inputX = Mathf.Clamp(xDiff, -1f, 1f);
        Run(-inputX);
    }

    public bool IsCloseToPlayer()
    {
        return (Physics2D.OverlapCircleAll(SwordPosition.position, AttackRange, PlayerLayer).Length > 0);
    }

    public void UpdateHealth()
    {
        BanditHealthSlider.value = BanditHealth;
        PlayerHealth = PlayerHealthSlider.value;
        if (BanditHealth <= 0) Death();
        if (PlayerHealth <= 0) _isPlayerDead = true;
    }

    public void Death()
    {
        m_animator.SetTrigger("Death");
        _isDead = true;
    }

    public void Hurt(float damage)
    {
        m_animator.SetTrigger("Hurt");
        BanditHealth -= damage;
    }

    public void Attack()
    {
        if (!_canAttack) return;
        ResetAttackTimer();

        Idle();
        m_animator.SetTrigger("Attack");
        if (IsSimulation) StartCoroutine(DamageAI());
        else StartCoroutine(DamagePlayer());

    }

    public void ResetAttackTimer()
    {
        _attackTimer = AttackCooldown;
        _canAttack = false;
    }

    public void TickAttackTimer()
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

    public void Run(float inputX)
    {
        SwapSpriteDirection(inputX);

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);
    }

    // Swap direction of sprite depending on walk direction
    public void SwapSpriteDirection(float xPos)
    {
        if (xPos > 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if (xPos < 0)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    public void Idle()
    {
        if (PlayerHealth > 0) m_animator.SetInteger("AnimState", 1);
        else ChillIdle();
    }

    public void ChillIdle()
    {
        m_animator.SetInteger("AnimState", 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(SwordPosition.position, AttackRange);
    }

    public bool ReachedMapEdge()
    {
        return (transform.position.x < LeftCollider.position.x + 2 || transform.position.x > RightCollider.position.x - 2);
    }

    public IEnumerator DamagePlayer()
    {
        yield return new WaitForSecondsRealtime(AttackAnimDelay);
        Collider2D[] player = Physics2D.OverlapCircleAll(SwordPosition.position, AttackRange, PlayerLayer);
        for (int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<Player>().Hurt(AttackDamage);
        }
    }

    public IEnumerator DamageAI()
    {
        yield return new WaitForSecondsRealtime(AttackAnimDelay);
        Collider2D[] player = Physics2D.OverlapCircleAll(SwordPosition.position, AttackRange, PlayerLayer);
        for (int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<Bandit>().Hurt(AttackDamage);
        }
    }

}
