using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [SerializeField] int maxHp = 3;
    [SerializeField] int damageOnHit = 1;
    [SerializeField] string Obstacle = "Obstacle";

    public int MaxHp => maxHp;
    public int Hp { get; private set; }

    void Awake() { Hp = maxHp; }

    public void TakeHit(int dmg)
    {
        Hp = Mathf.Max(0, Hp - Mathf.Max(0, dmg));
        if (Hp == 0) { /* TODO: 죽음 처리*/ }
    }

    void OnTriggerEnter2D(Collider2D other)  //부딪히면 깎기
    {
        if (other.CompareTag(Obstacle)) TakeHit(damageOnHit);
    }

}
