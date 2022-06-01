using UnityEngine;

namespace RPG.TimeOfDay
{
    public class TimeOfDay : MonoBehaviour
    {
        [SerializeField] private float timeSpeed = 0.005f;
        GameObject stars;
        Vector3 originalPosition;
        private float angle;


        private void Awake()
        {
            stars = GameObject.FindWithTag("Stars");
            originalPosition = transform.up;
        }
        void Update()
        {
            if (IsNight()) stars.SetActive(true);
            else stars.SetActive(false);
        }

        public bool IsNight()
        {
            transform.Rotate(transform.right, timeSpeed * Time.deltaTime);

            // regner ut *vinkelen MED fortegn* (sneaky og godt gjemt funksjon) mellom start vektor og current vektor.
            // rotert rundt rotasjonsaksen (V3.right)
            angle = Vector3.SignedAngle(originalPosition, transform.up, Vector3.right);

            //regner om fra relativ vinkel til absolutt vinkel slik at denne kan styre time of day.
            if (angle < 0) { angle = 360 - angle * -1; }

            // bestemmer natt og dag
            if (angle > 180 && angle < 360) { return true; }
            else { return false; }
        }
    }
}

