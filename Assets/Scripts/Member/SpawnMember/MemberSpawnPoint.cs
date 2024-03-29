using UnityEngine;

namespace Member.SpawnMember
{
    public class MemberSpawnPoint : MonoBehaviour
    {
        private void Awake() => MemberSpawnSystem.AddSpawnPoint(transform);

        private void OnDestroy() => MemberSpawnSystem.RemoveSpawnPoint(transform);

        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward *2);
        }*/
    }
}
