using System.Collections;
using Behaviour;
using UnityEngine;
using UnityEngine.AI;

public class WanderingEnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private WanderingEnemyFXController wanderingEnemyFXController;
    private Vector3 targetPosition;

    public Animator animator;
    public float speed = 3f;
    public float smoothRotationSpeed = 5f;
    public float attackDistance = 1.5f;

    private bool attacking = false;
    private bool attackCooldown = false;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        wanderingEnemyFXController = GetComponentInChildren<WanderingEnemyFXController>();

        if (navMeshAgent == null)
        {
            enabled = false;
            return;
        }

        navMeshAgent.speed = speed;
        SetRandomDestination();
    }

    private void FixedUpdate()
    {
        if (!attacking && !attackCooldown && (Vector3.Distance(transform.position, targetPosition) < attackDistance || !navMeshAgent.hasPath))
        {
            SetRandomDestination();
            MoveAndRotateTowardsDestination();
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, 1))
        {
            targetPosition = hit.position;
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    private void MoveAndRotateTowardsDestination()
    {
        if (navMeshAgent.hasPath && navMeshAgent.desiredVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attacking && !attackCooldown && other.CompareTag("Rabbit"))
        {
            StartCoroutine(AttackRabbit(other));
        }
    }

    private IEnumerator AttackRabbit(Collider other)
    {
        attacking = true;
        attackCooldown = true;

        navMeshAgent.isStopped = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/SwampGolem/Attack");
        wanderingEnemyFXController.attacking = true;
        animator.SetTrigger("AttackTrigger");

        yield return new WaitForSeconds(1f);

        if (other != null && other.gameObject.activeSelf)
        {
            WanderScript wanderScript = other.GetComponent<WanderScript>();
            if (wanderScript != null)
            {
                wanderScript.Die();
            }
        }

        yield return new WaitForSeconds(2f);

        wanderingEnemyFXController.attacking = false;
        navMeshAgent.isStopped = false;
        attacking = false;

        yield return new WaitForSeconds(5f);
        attackCooldown = false; // Reset attack cooldown after 5 seconds
    }
}
