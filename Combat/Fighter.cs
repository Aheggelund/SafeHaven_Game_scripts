using UnityEngine;
using TMPro;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenKicks = 2.5f;
        [SerializeField] float maxDamage = 10f;
        [SerializeField] float minDamage = 2f;


        Health target;
        Mover agentMover;
        Animator animator;

        float timeSinceLastAttack = Mathf.Infinity;
        public GameObject damageTextPrefab;
        public string textToDisplay;

        private void Awake()
        {
            agentMover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {


            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                agentMover.MoveTo(target.transform.position);
            }
            else
            {
                agentMover.Cancel();
                AttackBehaviour();

            }
        }

        private void AttackBehaviour()
        {

            transform.LookAt(target.transform);

            if (timeSinceLastAttack > timeBetweenKicks)
            {
                // this will trigger the Hit() event.
                animator.ResetTrigger("cancelCombat");
                animator.SetTrigger("kick");
                timeSinceLastAttack = 0;
            }
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;
            DoDamage();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            return combatTarget != null && !combatTarget.GetComponent<Health>().IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            animator.SetTrigger("cancelCombat");
        }

        public void DoDamage()
        {
            float damage = Random.Range(minDamage, maxDamage);
            target.TakeDamage(damage);

            // Adding floating damage text
            string dmgText = Mathf.Round(damage).ToString();
            GameObject damageTextInstance = Instantiate(damageTextPrefab, target.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(Camera.main.transform.forward));
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(dmgText);
        }
    }
}

