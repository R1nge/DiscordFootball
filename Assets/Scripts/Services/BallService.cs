using UnityEngine;

namespace Services
{
    public class BallService
    {
        public void SaveReplay(Rigidbody rigidbody, Vector3 currentPosition, out Vector3 replayPosition)
        {
            rigidbody.velocity = Vector3.zero;
            replayPosition = currentPosition;
        }

        public void PlayReplay(Rigidbody rigidbody, Vector3 replayPosition, Transform playerTransform)
        {
            rigidbody.velocity = Vector3.zero;
            playerTransform.position = replayPosition;
        }
    }
}