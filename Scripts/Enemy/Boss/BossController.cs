using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;
    BossHealthSystem bossHealth;
    Animator anim;
    public float shortRange;
    public float timeStartBattle;
    public BoxCollider2D box;
    public LayerMask whatIsPlayer;
    [SerializeField] GameObject stunEffect;
    //porcentajes
    public int [] shortPercents = new int[4];
    public int [] longPercents = new int[4];
    int[] percentatgeArray = new int [100];
    int lastAttack;
    public int sameAttackPercent;
    public string meleAttack;
    public string dashAttack;
    public string waveAttack;
    public string waveRageAttack;
    public string cageAttack;
    private void Awake() {
        instance = this;
    }

    private void Start() {
        lastAttack = 3;
        anim = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealthSystem>();
        StartCoroutine(FirstMove());
    }

    IEnumerator FirstMove()
    {
        yield return new WaitForSeconds(timeStartBattle);
        ChooseAttack();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(box.bounds.center, shortRange);
    }

    public void ChooseAttack()
    {
        FlipToPlayer();
        PickAttack();
    }

    void FlipToPlayer()
    {
        if(BossAttackDash.instance.playerBox.transform.position.x < box.transform.position.x)
        {
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
        else if(BossAttackDash.instance.playerBox.transform.position.x > box.transform.position.x)
        {
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }

    void PickAttack()
    {
        if(IsInRange(shortRange))
        {
            GetAttack(shortPercents);
        }
        else
        {
            GetAttack(longPercents);
        }
    }

    void GetAttack(int[] arrays)
    {
        int[] percentages = new int[4];
        int attackPicked;
        for (int i = 0; i < arrays.Length; i++)
        {
            percentages[i] = arrays[i];
        }
        
        if(!bossHealth.rage)
        {
            //repartir la probabilidad del ataque de la cage entre los otros ataques, ya que no se tiene en cuenta pq el boss no puede hacer ese ataque
            percentages = SharePoints(percentages);
        }
        //computar i elegir ganador
        string debug ="";
        ComputePercentage(percentages);
        attackPicked = PickWinner(percentages);
        //
        for (int i = 0; i < percentages.Length; i++)
        {
            debug = debug + i.ToString() + ": " + percentages[i].ToString() + "%  ,";
        }
        debug = debug + "  winner = " + attackPicked;
        //
        Debug.Log(debug);
        ChooseAnimation(attackPicked);
    }

    int[] SharePoints(int[] array)
    {
        bool[] affected = new bool[3];
        int count = 0;
        int pointsToShare = array[3];
        
        //mirar a que ataques se les tiene que sumar probabilidad
        for (int i = 0; i < affected.Length; i++)
        {
            if(array[i] > 0)
            {
                affected[i] = true;
                count++;
            }
        }
        //sumarles la probabilidad que les pertoca
        for (int i = 0; i < affected.Length; i++)
        {
            if(affected[i])
            {
                array[i] += pointsToShare/count;
            }
        }
        //poner la probabilidad del ataque cage a 0, ya que ya se a repartido su probabilidad entre los otros
        array[3] = 0;

        return array;
    }
    void ChooseAnimation(int attack)
    {
        switch (attack)
        {
            case 0:
            anim.Play(meleAttack);
            break;

            case 1:
            anim.Play(dashAttack);
            break;

            case 2:
            if(bossHealth.rage)
            {
                anim.Play(waveRageAttack);
            }
            else
            {
                anim.Play(waveAttack);
            }
            break;

            case 3:
            anim.Play(cageAttack);
            break;

            default:
            break;
        }
    }

    public void EnableStunEffect(bool state)
    {

        stunEffect.SetActive(state);
        
    }

    void ComputePercentage(int[] posibilities)
    {
        percentatgeArray = new int[100];
        int index = 0;
        for (int i = 0; i < posibilities.Length; i++)
        {
            for (int j = 0; j < posibilities[i]; j++)
            {
                percentatgeArray[index] = i;
                index++;
            }            
        }
    }
    int GetFirstEmptySlot()
    {
        for (int i = 0; i < percentatgeArray.Length; i++)
        {
            if(percentatgeArray[i] == 0)
            {
                return i;
            }
        }
        return 0;
    }

    int PickWinner(int[] attacks)
    {
        int attack;
        int rnd = Random.Range(1,100);
        attack = percentatgeArray[rnd];
        if(Random.Range(1,100) <= sameAttackPercent && attacks[lastAttack] != 0)
        {
            attack = lastAttack;
        }
        else
        {
            while(attack == lastAttack)
            {
                rnd = Random.Range(1,100);
                attack = percentatgeArray[rnd];
            }
        }
        //si el ataque es el mismo que el ultimo, se vuelve a escojer uno, hasta que se tiene uno nuevo
        lastAttack = attack;
        return attack;
    }

    bool IsInRange(float range)
    {
        Collider2D player = Physics2D.OverlapCircle(box.bounds.center, range, whatIsPlayer);
        return player != null;
    }
}
