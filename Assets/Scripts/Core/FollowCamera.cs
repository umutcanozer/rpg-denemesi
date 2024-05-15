using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        void LateUpdate()
        {
            FollowingCamera();
        }

        void FollowingCamera()
        {
            transform.position = target.position;
        }
    }
}
