using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        Animator animator;
        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0 && !isDead)
            {
                Dying();

            }
        }

        private void Dying()
        {
            isDead = true;
            animator.SetTrigger("isDead");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}