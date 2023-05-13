using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Member
{
    public class NavMeshMovement : MonoBehaviour
    {
        public NavMeshAgent agent;
        public NavMeshPath NavMeshPath;

        public List<Vector3> pathPoints;
        public List<Vector3> randomizedPath;

        public int currentIndex = 0;
        public Vector3 currentTarget;

        public float randomRadius = 5f;
        public float randomRadiusEnd = 1f;
        public float validClickRange = 10f;
        public bool arrived = false;

        private void Awake()
        {
            agent.updateRotation = false;
        }

        void Update()
        {
            if (randomizedPath.Count > 0 && currentIndex < randomizedPath.Count)
            {
                Vector3 flatAgentPos = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 flatTargetPos = new Vector3(currentTarget.x, 0, currentTarget.z);

                if (Vector3.Distance(flatAgentPos, flatTargetPos) <= agent.stoppingDistance * 1.1f)
                {
                    if (currentIndex < randomizedPath.Count - 1)
                    {
                        currentIndex++;
                        GoToCurrentTarget();
                    }
                    else
                    {
                        arrived = true;
                        currentIndex = 0;
                        ResetPaths();
                    }
                }
            }
        }

        public void GoToDestination(Vector3 destination)
        {
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, validClickRange, NavMesh.AllAreas) &&
                !IsPointerOverUIObject())
            {
                arrived = false;
                CalculatePath(hit.position);
            }
            else
            {
                ResetPaths();
                Debug.Log("INVALID POSITION");
            }
        }

        public void CalculatePath(Vector3 destination)
        {
            ResetPaths();
            NavMeshPath = new NavMeshPath();
            agent.CalculatePath(destination, NavMeshPath);

            for (int i = 1; i < NavMeshPath.corners.Length; i++)
            {
                pathPoints.Add(NavMeshPath.corners[i]);

                if (i < NavMeshPath.corners.Length - 1)
                {
                    bool validPos = false;
                    Vector3 randomizedDestination = Vector3.zero;
                    while (!validPos)
                    {
                        randomizedDestination = NavMeshPath.corners[i] + Random.insideUnitSphere * randomRadius;
                        randomizedDestination.y = NavMeshPath.corners[i].y;
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(randomizedDestination, out hit, 1.0f, NavMesh.AllAreas))
                        {
                            randomizedDestination = hit.position;
                            validPos = true;
                        }
                    }
                    randomizedPath.Add(randomizedDestination);
                }
                else
                {
                    bool validPos = false;
                    Vector3 randomizedDestination = Vector3.zero;
                    while (!validPos)
                    {
                        randomizedDestination = NavMeshPath.corners[i] + Random.insideUnitSphere * randomRadiusEnd;
                        randomizedDestination.y = NavMeshPath.corners[i].y;
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(randomizedDestination, out hit, 1.0f, NavMesh.AllAreas))
                        {
                            randomizedDestination = hit.position;
                            validPos = true;
                        }
                    }

                    randomizedPath.Add(randomizedDestination);
                }
            }
            GoToCurrentTarget();
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        void ResetPaths()
        {
            randomizedPath.Clear();
            pathPoints.Clear();
            currentIndex = 0;
        }

        void GoToCurrentTarget()
        {
            agent.SetDestination(randomizedPath[currentIndex]);
            currentTarget = randomizedPath[currentIndex];
        }
    }
}