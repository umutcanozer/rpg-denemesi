using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        bool isDied = false;
        Animator _anim;

        public bool IsDied
        {
            get { return isDied; }
        }

        private void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health-damage,0);
            print(health);
            if (health == 0)
            {
                Die();
            }

        }

        private void Die()
        {
            if (isDied) return;
            isDied = true;
            _anim.SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
        }
    }
}
