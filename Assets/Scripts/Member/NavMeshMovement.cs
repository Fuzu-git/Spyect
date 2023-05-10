using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

        private void Awake()
        {
            agent.updateRotation = false;
        }

        void Update()
        {
            if (randomizedPath.Count > 0 && currentIndex < randomizedPath.Count)
            {
                if (Mathf.Abs(transform.position.x - currentTarget.x) < 1f &&
                    Mathf.Abs(transform.position.z - currentTarget.z) < 1f)
                {
                    if (currentIndex < randomizedPath.Count - 1)
                    {
                        currentIndex++;
                        GoToCurrentTarget();
                    }
                    else
                    {
                        currentIndex = 0;
                        ResetPaths();
                    }
                }
            }
        }

        public void GoToDestination(Vector3 destination)
        {
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, validClickRange, NavMesh.AllAreas))
            {
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
            Debug.Log($"path length {NavMeshPath.corners.Length}");

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