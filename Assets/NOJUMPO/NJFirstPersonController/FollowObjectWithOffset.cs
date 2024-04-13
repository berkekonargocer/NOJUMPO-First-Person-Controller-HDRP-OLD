using UnityEngine;

namespace NOJUMPO.Tools
{
    public class FollowObjectWithOffset : MonoBehaviour
    {
        // -------------------------------- FIELDS ---------------------------------
        [SerializeField] GameObject objectToFollow;
        [SerializeField] float followSpeed = 5.0f;
        Vector3 _offset;

        // ------------------------- UNITY BUILT-IN METHODS ------------------------
        void Awake() {
            _offset = transform.position - objectToFollow.transform.position;
        }

        void LateUpdate() {
            Transform objectTransform = transform;
            objectTransform.position = objectToFollow.transform.position + _offset;
            transform.rotation = Quaternion.Slerp(objectTransform.rotation, objectToFollow.transform.rotation, followSpeed * Time.deltaTime);
        }
    } 
}