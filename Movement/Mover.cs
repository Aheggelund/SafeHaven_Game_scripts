using UnityEngine;
using UnityEngine.AI;

using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent agent;
        private Animator animator;
        private Health health; 

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void startMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;

        }

        public void Cancel()
        {
            agent.isStopped = true;
        }
        private void UpdateAnimator()
        {
            // useful function: InverseTransformDirection. Converts from global to local frame of reference.
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            animator.SetFloat("forwardSpeed", localVelocity.z);
        }
    }

}
