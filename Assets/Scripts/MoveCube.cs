using System.Collections;
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
    public float delay = 0f;

    public List<Transform> targets;
    public List<Transform> directions;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo))
            {
                CalculateAndShowPath(hitinfo.point);
                //StartCoroutine(SetDestination());
            }
        }

        //Debug.Log(Mathf.Abs(transform.position.x - currentTarget.x));

        /*if (Mathf.Abs(transform.position.x - currentTarget.x) < 2f && Mathf.Abs(transform.position.z - currentTarget.z) < 2f)
        {
            StartCoroutine(SetDestination());
        }*/

        if (pathPoints.Count > 0 && currentIndex < pathPoints.Count)
        {
            if (Mathf.Abs(transform.position.x - currentTarget.x) < 1f && Mathf.Abs(transform.position.z - currentTarget.z) < 1f)
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

    IEnumerator SetDestination()
    {
        yield return new WaitForSeconds(delay);
        Vector3 pos = targets[Random.Range(0, targets.Count)].position;
        agent.SetDestination(pos);
        currentTarget = pos;
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
        Vector3 mainTarget = pathPoints[currentIndex].transform.position;
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
            List<Vector3> diagResult = new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

            //je calcule deux points intermédaires pour dessiner mes deux droites de support
            Vector3 tempTarget1 = new Vector3(pathPoints[currentIndex].transform.position.x, transform.position.y, transform.position.z);
            Vector3 tempTarget2 = new Vector3(transform.position.x, transform.position.y, pathPoints[currentIndex].transform.position.z);

            List<Vector3> diagResults = new List<Vector3>();

            Vector3 diag1;
            Vector3 diag1_v2;
            bool test1 = LineLineIntersection(out diag1, transform.position, directions[1].forward, tempTarget1, mainTarget - tempTarget1);
            if (test1) diagResult.Add(diag1);
            bool test2 = LineLineIntersection(out diag1_v2, transform.position, directions[1].forward, tempTarget2, mainTarget - tempTarget2);
            if (test2) diagResult.Add(diag1_v2);

            Vector3 diag2;
            Vector3 diag2_v2;
            bool test3 = LineLineIntersection(out diag2, transform.position, directions[3].forward, tempTarget1, mainTarget - tempTarget1);
            if (test3) diagResult.Add(diag2);
            bool test4 = LineLineIntersection(out diag2_v2, transform.position, directions[3].forward, tempTarget2, mainTarget - tempTarget2);
            if (test4) diagResult.Add(diag2_v2);

            Vector3 diag3;
            Vector3 diag3_v2;
            bool test5 = LineLineIntersection(out diag3, transform.position, directions[5].forward, tempTarget1, mainTarget - tempTarget1);
            if (test5) diagResult.Add(diag3);
            bool test6 = LineLineIntersection(out diag3_v2, transform.position, directions[5].forward, tempTarget2, mainTarget - tempTarget2);
            if (test6) diagResult.Add(diag3_v2);

            Vector3 diag4;
            Vector3 diag4_v2;
            bool test7 = LineLineIntersection(out diag4, transform.position, directions[7].forward, tempTarget1, mainTarget - tempTarget1);
            if (test7) diagResult.Add(diag4);
            bool test8 = LineLineIntersection(out diag4_v2, transform.position, directions[7].forward, tempTarget2, mainTarget - tempTarget2);
            if (test8) diagResult.Add(diag4_v2);

            float minDistance = float.PositiveInfinity;
            Vector3 currentTempTarget = Vector3.zero;
            foreach (Vector3 resultat in diagResult)
            {
                float distance1 = Vector3.Distance(resultat, mainTarget);
                float distance2 = Vector3.Distance(resultat, transform.position);

                if (distance1 + distance1 < minDistance)
                {
                    minDistance = distance1 + distance2;
                    currentTempTarget = resultat;
                }
            }

            //res est le point en diagonal selectionné
            if (TestAndGo(currentTempTarget)) return;
            if (TestAndGo(tempTarget1)) return;
            if (TestAndGo(tempTarget2)) return;
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
        //agent.SetDestination(target);
        GameObject point = Instantiate(pathpointPfb, target, Quaternion.identity, Camera.transform);
        point.GetComponent<Renderer>().material.color = Color.red;
        return true;
    }

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
}
