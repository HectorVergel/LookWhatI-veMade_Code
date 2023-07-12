using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreDashState : MonoBehaviour, SMCInterface
{
    public string stateName;
    ShieldEnemyController controller;
    public SMCInterface _nextState { get => GetComponent<DashState>();}
    private void Start() {
        controller = GetComponent<ShieldEnemyController>();
    }

    public void OnEnter()
    {
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
