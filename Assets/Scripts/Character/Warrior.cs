using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayableCharacter
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private AudioSource fireballSound;
    private float coolDownTimer = Mathf.Infinity;
    int fireballIndex = 0;

    new private void Start()
    {
        base.Start();
        fireballSound = GameObject.Find("FireballSound").GetComponent<AudioSource>();
    }
    public override void SpecialAbility()
    {
        if (canAttack())
        {
            Attack();
        }
    }
    private bool canAttack()
    {
        bool isCoolDown = coolDownTimer < attackCooldown;
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isGrounded = horizontalInput == 0 && playerMovement.Grounded();

        return isGrounded && !isCoolDown;
    }

    void Update()
    {
        coolDownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        playerMovement.GetAnimator().SetTrigger("attack");
        coolDownTimer = 0;

        int fireballIndex = FindFireball();
        if (fireballIndex != -1) // remove the if
        {
            fireballs[fireballIndex].transform.position = firePoint.position;
            fireballs[fireballIndex].SetActive(true);
            fireballs[fireballIndex].GetComponent<Projectile>().StartShooting(Mathf.Sign(transform.localScale.x));

            fireballSound.Play();
        }
    }

    private int FindFireball()  // chanage return value
    {
        fireballIndex++;
        if (fireballIndex >= 9) 
        {
            fireballIndex = 0;

        }

        return fireballIndex;
    }

}

