using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float currentHealth;
    private float maxHealth = 10f;
    private bool isDead = false;

    private Transform targetTransform;
    private PlayerController targetScript;
    private bool onCooldown;
    private float basicAttackCooldown = 2f;

    private int moveSpeed = 1;
    private int rotationSpeed = 3;

    private int damage = 5;
    private float baseProgression = 0.5f;

    private float noticeDistance = 10f;
    private float attackDistance = 2f;

    private Transform myTransform;
    private Animator anim;

    public void Initialize()
    {
        myTransform = transform;

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        anim = GetComponent<Animator>();

        currentHealth = maxHealth;

        StartCoroutine(HandleMovement());
    }

    private IEnumerator HandleMovement()
    {
        while (true)
        {
            float distance = (targetTransform.transform.position - transform.position).magnitude;
            float direction = Vector3.Dot((targetTransform.position - transform.position).normalized, transform.forward);

            if (distance < noticeDistance && distance > attackDistance)
            {
                Rotate();

                Move();
            }
            else if (distance <= attackDistance && direction > 0 && !onCooldown)
            {
                Attack();
                StartCoroutine(WaitForCooldown(basicAttackCooldown));
            }
            else
            {
                anim.SetFloat("MoveZ", 0f);
            }

            yield return null;
        }

        yield break;
    }

    private void Rotate()
    {
        // If not looking at target, look at target. 
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetTransform.position - myTransform.position), rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        // If target is not reached, move towards target.
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        anim.SetFloat("MoveZ", 1f);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        targetScript.AdjustCurrentHealth(damage);
    }

    private IEnumerator WaitForCooldown(float cooldownTime)
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        onCooldown = false;

        yield break;
    }

    public void AdjustCurrentHealth(float adj)
    {
        if (isDead) return;

        currentHealth -= adj;

        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            Die();
        }

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    private void Die()
    {
        anim.SetBool("Dead", true);
        GameManager.Instance.ActiveCharacterInformation.AddExperiencePoints(baseProgression);

        StartCoroutine(WaitForDestory());
    }

    private IEnumerator WaitForDestory()
    {
        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);

        yield break;
    }
}
