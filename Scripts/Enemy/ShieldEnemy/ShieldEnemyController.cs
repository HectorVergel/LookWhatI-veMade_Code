using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SMCInterface
{
    SMCInterface _nextState{get;}
    public void OnEnter();
    public void OnUpdate();
    public void OnExit();

}
public class ShieldEnemyController : MonoBehaviour
{
    [System.NonSerialized]
    public bool canBeHitted;
    public SMCInterface _currentState;
    WalkState walkState;
    PreDashState preDashState;
    DashState dashState;
    EndDashState  endDashState;
    [System.NonSerialized]
    public BoxCollider2D target;
    BoxCollider2D myCollider;
    [System.NonSerialized]
    public Animator anim;
    public GameObject sound;

    public ParticleSystem vfx1;
    public ParticleSystem vfx2;
     
    void Start()
    {
        anim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        GetPlayer();
        walkState = GetComponent<WalkState>();
        preDashState = GetComponent<PreDashState>();
        dashState = GetComponent<DashState>();
        endDashState = GetComponent<EndDashState>();
        ChangeCurrentState(walkState);
    }

    void Update()
    {
        _currentState.OnUpdate();
        if(target != null)
        {
            canBeHitted = CheckIfFacingPlayer();
        }
        else
        {
            GetPlayer();
        }
        
    }

    public void ChangeCurrentState(SMCInterface _nextState)
    {
        _currentState = _nextState;
        _currentState.OnEnter();
    }
    
    public bool CheckIfFacingPlayer()
    {
        Vector2 direction = target.bounds.center - myCollider.bounds.center;
        bool enemyFacingRight = transform.right.x > 0;
        bool playerOnRight = direction.normalized.x > 0;
        return enemyFacingRight != playerOnRight;
    }
    public void ShieldHit()
    {
        GetComponent<PrintDamage>().ShowText(0, false);
        vfx1.Play();
        vfx2.Play();
        Instantiate(sound,transform.position,Quaternion.identity);
    }
    public void GetPlayer()
    {
        if(FindObjectOfType<HealthSystemPlayer>() != null)
        {
            target = FindObjectOfType<HealthSystemPlayer>().GetComponent<BoxCollider2D>();
        }
    }
}
