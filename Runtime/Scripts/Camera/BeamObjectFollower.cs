using UnityEngine;

namespace BeamXR.Director.Camera
{
    public class BeamObjectFollower : MonoBehaviour
    {
        public Transform transformToFollow;

        public bool trackPosition = true, trackRotation = true;

        public Vector3 positionOffset;

        public void Update()
        {
            if(transformToFollow != null)
            {
                if (trackPosition)
                {
                    Quaternion rotation = transformToFollow.rotation;
                    rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                    transform.position = transformToFollow.position + (rotation * positionOffset);
                }

                if (trackRotation)
                {
                    transform.LookAt(transformToFollow);
                }
            }
        }
    }
}