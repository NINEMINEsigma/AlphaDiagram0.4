using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD.Game
{
    public class Bullet : MonoBehaviour
    {
        public Transform Parent;

        private void Update()
        {
            if (Parent == null || Vector3.Distance(transform.position, Parent.position) > 36)
            {
                GameObject.Destroy(gameObject);
                return;
            }
            transform.Translate(Vector3.up * Time.deltaTime * 7.5f, Space.Self);
        }
    }
}