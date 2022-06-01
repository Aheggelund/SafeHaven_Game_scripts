using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        private GameObject player;
        private Health health;

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 10f;
        [SerializeField] PatrolPath patrolPath;

        //distance between a
        private Vector3 separation;
        private Vector3 originalPosition;

        private float timeSinceLastSawPlayer = Mathf.Infinity;
        
        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            patrolPath = GetComponent<PatrolPath>();
            originalPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer > suspicionTime) 
            {
                PatrolBehaviour();
            }
            else
            {
                SuspicionBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            // Get the vector instead of .Distance() to have the possibility for FOV trigger
            separation = player.transform.position - transform.position;
            float angle = Vector3.Angle(separation, transform.forward);
            return separation.magnitude < chaseDistance;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = originalPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint(); 
            }

            mover.startMoveAction(nextPosition);
        }

        private bool AtWaypoint()
        {
            throw new NotImplementedException();
        }

        private void CycleWaypoint()
        {
            throw new NotImplementedException();
        }

        private Vector3 GetCurrentWaypoint()
        {
            throw new NotImplementedException();
        }

        // Called by Unity
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

