using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAnimation : MonoBehaviour {

    [SerializeField]
    Entity entity;
    [SerializeField]
    Animator animator;
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    EntityAttack entityAttack;
    [SerializeField]
    EntityGatherer gatherer;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    EntityLookAt lookAt;

    void Start()
    {
        // Don’t update position automatically
        agent.updatePosition = false;
        lookAt = GetComponent<EntityLookAt>();
        animator.SetFloat("attackSpeed", entityAttack.Stats.AttackSpeed);
    }

    private void OnEnable()
    {
        entity.OnEntityDeath += Entity_OnDeath;
        entityAttack.AttackControl.OnAttackStart += AttackControl_OnAttackStart;
    }

    private void AttackControl_OnAttackStart(Entity target)
    {
        animator.SetTrigger("attack");
    }

    private void Entity_OnDeath()
    {
        animator.SetTrigger("dead");
    }

    private void OnDisable()
    {
        entity.OnEntityDeath -= Entity_OnDeath;
        entityAttack.AttackControl.OnAttackStart -= AttackControl_OnAttackStart;
    }

    void Update()
    {
        if (lookAt)
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

        if (gatherer)
        {
            animator.SetBool("gather", gatherer.Gathering());
        }

        if (!agent.enabled)
        {
            animator.SetBool("move", false);
            return;
        }

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0f && agent.remainingDistance > agent.radius / 2f;

        // Update animation parameters
        animator.SetBool("move", shouldMove);
        animator.SetFloat("speedx", velocity.x);
        animator.SetFloat("speedz", velocity.y);

        //GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
       
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }
}
