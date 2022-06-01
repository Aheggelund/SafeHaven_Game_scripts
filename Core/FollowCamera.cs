using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        private Vector3 offset;

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}

