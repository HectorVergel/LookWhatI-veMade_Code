using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDashState : MonoBehaviour, SMCInterface
{
    public string stateName;
    ShieldEnemyController controller;
    Rigidbody2D rb;
    public SMCInterface _nextState { get => GetComponent<WalkState>();}
    private void Start() {
        controller = GetComponent<ShieldEnemyController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        rb.velocity = new Vector2(0,rb.velocity.y);
        controller.anim.Play(stateName);
    }

    public void OnExit()
    {
        controller.ChangeCurrentState(_nextState);
    }

    public void OnUpdate()
    {

    }
}
