using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCube : MonoBehaviour
{
    public NavMeshAgent agent;
    public Camera Camera;
    public List<GameObject> pathPoints;
    public int currentIndex = 0;
    public GameObject pathpointPfb;

    public NavMeshPath finalPath;
    public Vector3 currentTarget;
    public bool isMainPoint = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo))
            {
                CalculateAndShowPath(hitinfo.point);
                //agent.SetDestination(hitinfo.point);
            }
        }

        if (pathPoints.Count > 0 && currentIndex < pathPoints.Count)
        {
            if (Mathf.Abs(transform.position.x - currentTarget.x) < 0.001f && Mathf.Abs(transform.position.z - currentTarget.z) < 0.001f)
            {
                if (isMainPoint)
                {
                    currentIndex++;
                    isMainPoint = false;
                }
                if (currentIndex < pathPoints.Count)
                {
                    SelectViableDestinations();
                }
            }
        }
    }

    public void CalculateAndShowPath(Vector3 destination)
    {
        finalPath = new NavMeshPath();
        agent.CalculatePath(destination, finalPath);

        foreach (GameObject go in pathPoints)
        {
            Destroy(go);
        }
        pathPoints.Clear();

        for (int i = 1; i < finalPath.corners.Length; i++)
        {
            GameObject go = Instantiate(pathpointPfb, finalPath.corners[i], Quaternion.identity, Camera.transform);
            pathPoints.Add(go);
            go.name = $"Point {i}";
        }

        SelectViableDestinations();
    }

    public void SelectViableDestinations()
    {
        Vector3 currentTargetDirection = pathPoints[currentIndex].transform.position - transform.position;
        //si la cible est sur une destination qui ne demande pas de tourner
        if (Mathf.Approximately(currentTargetDirection.x, 0f) || Mathf.Approximately(currentTargetDirection.z, 0f) || Mathf.Approximately(Mathf.Abs(currentTargetDirection.x), Mathf.Abs(currentTargetDirection.z)))
        {
            //go to current target
            agent.SetDestination(pathPoints[currentIndex].transform.position);
            currentTarget = pathPoints[currentIndex].transform.position;
            isMainPoint = true;
        }
        else
        {
            //je calcule un point intermédaire qui pêut-être valide
            Vector3 tempTarget = new Vector3(pathPoints[currentIndex].transform.position.x, transform.position.y, transform.position.z);
            //je vérifie qu'il n'y a pas d'obstables m'empechant de l'atteindr ene ligne droite
            if (!TestAndGo(tempTarget))
            {
                tempTarget = new Vector3(pathPoints[currentIndex].transform.position.x, transform.position.y, transform.position.z);
                TestAndGo(tempTarget);
            }
        }
    }

    public bool TestAndGo(Vector3 target)
    {
        NavMeshPath testPath = new NavMeshPath();
        agent.CalculatePath(target, testPath);
        if (testPath.corners.Length > 2) //y'a un obstacle il faut calculer de nouveaux point intermédiaire
        {
            return false;
        }
        currentTarget = target;
        agent.SetDestination(target);
        return true;
    }
}
