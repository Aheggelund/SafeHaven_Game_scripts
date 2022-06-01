using UnityEngine;

namespace RPG.Core
{
    public class DestroyOnAnimationEnd : MonoBehaviour
    {
        public void DestroyParent()
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            Destroy(parent);
        }
        
    }
}