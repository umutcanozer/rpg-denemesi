using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float range = 2f;
        [SerializeField] float timeBeetweenAttacks = 1.5f;
        [SerializeField] float attackDamage = 10f;

        float timeSinceLastAttack = 0f;
        Health target;
        Animator _animator;

        Mover _mover;
        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDied == true) return;
            if (!GetIsInRange())
            {
                _mover.MoveTo(target.transform.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }

        }

        //Animation event
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(attackDamage);
        }


        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBeetweenAttacks)
            {
                //This will trigger the hit event.
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }

        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < range;
        }

        public void Attack(GameObject combat)
        {
            target = combat.GetComponent<Health>();               
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDied;
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }


    }
}

