using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
         

        GameObject player;

        Health _health;
        Fighter _fighter;
        Mover _mover;

        float timeSinceLastSaw = Mathf.Infinity;
        Vector3 guardLocation;
        int currentWaypointIndex = 0;
        void Start()
        {
            guardLocation = transform.position;
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");

            _mover = GetComponent<Mover>();
        }


        
        void Update()
        {

            if (_health.IsDied) return;
        
            if (InRange() && _fighter.CanAttack(player))
            {
                timeSinceLastSaw = 0;
                AttackBehaviour();
            }
            else if(timeSinceLastSaw < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeSinceLastSaw += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation;

            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            _mover.StartMoveAction(nextPosition);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _fighter.Attack(player);
        }

        bool InRange()
        {
            float v = Vector3.Distance(player.transform.position, transform.position);
            return v < chaseDistance;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,chaseDistance);
        }
    }
}
