using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        NavMeshAgent nma;

        Health health;
        Animator _anim;



        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            nma = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
            nma.enabled = !health.IsDied;
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        public void MoveTo(Vector3 _destination)
        {
            nma.destination = _destination;
            nma.isStopped = false;
        }

        public void Cancel()
        {
            nma.isStopped = true;
        }

       
        void UpdateAnimator()
        {
            Vector3 _velocity = nma.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(_velocity);

            float speed = localVelocity.z;
            _anim.SetFloat("forwardSpeed", speed);
            //Debug.Log(localVelocity + " " + nma.velocity);
        }
    }
}
