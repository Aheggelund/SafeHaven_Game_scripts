using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        private Health health;
        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead()) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().SetTrigger("DANCE");
            }

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            print("Here we can do somethin!");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(1))
                {
                    fighter.Attack(target.gameObject);
                }

                return true; // enables cursor change to imply combat target or not. 
            }

            return false;
        }

        private bool InteractWithMovement()
        {
           
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.startMoveAction(hit.point);
                }
                return true; 
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
