using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrypt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Knife;
    public bool CanAttack = true;
    public float attackCooldown = 0.5f;
    public bool IsAttacking = false;
    public int damage = 994; 
    //public int Sanimation;
    // The maximum distance away from the explosion tanks can be and are still affected 
    public float m_ExplosionRadius = 9; 
    // The amount of force added to a tank at the centre of the explosion 
    public float m_ExplosionForce = 60f;       

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanAttack)
            {
                SwordAttack();
            }
        }

    }

    private void OnTriggerEnter(Collider other) 
    {

        if(IsAttacking)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
            TankHealth targetHealth = other.gameObject.GetComponent<TankHealth>(); 
                if (targetHealth != null) 
                {
                    targetHealth.TakeDamage(damage); 
                } 
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                Shell bullet = other.gameObject.GetComponent<Shell>(); 
                bullet.OnDeath();
            }
        }
    }
    public void SwordAttack()
    {

        IsAttacking = true;
        CanAttack = false;
        Animator anim = Knife.GetComponent<Animator>();
        anim.SetTrigger("Swing");
        StartCoroutine(ResetAttackCooldown());
    }
    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(attackCooldown);
        CanAttack = true;
    }
    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(attackCooldown);
        IsAttacking = false;
    }
}
