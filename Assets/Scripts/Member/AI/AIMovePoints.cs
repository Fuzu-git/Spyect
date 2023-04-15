using UnityEngine;

namespace Member.AI
{
    public class AIMovePoints : MonoBehaviour
    {
        private void Awake() => AIBehaviour.AddMovePoint(transform);

        private void OnDestroy() => AIBehaviour.RemoveMovePoint(transform);

        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(transform.position, 1.25f);
            Gizmos.color = Color.red;
            //Gizmos.DrawLine(transform.position, transform.position + transform.forward *2);
        }*/
    }
}
