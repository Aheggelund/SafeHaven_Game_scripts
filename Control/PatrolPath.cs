using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [ExecuteInEditMode]
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointSize = 0.5f;
        [SerializeField] int numberOfWaypoints = 5;
        [SerializeField] float pathFluctuation = 3f;
        [SerializeField] float patrolPerimeter = 10f; //Vector2 patrolArea;

        private GameObject waypoint;
        private List<GameObject> waypoints;

        private float x_position;
        private float y_position;
        private float z_position;

        private void Awake()
        {
            this.transform.position = transform.parent.position;
            waypoints = new List<GameObject>();
            // Genererer et tomt objekt for å kunne klone det i GenerateWaypoints()

            // waypoint = new GameObject("Waypoint");
            // waypoint.transform.position = transform.position;
            // waypoint.transform.SetParent(transform);
        }
        private void Update()
        {

            if (waypoints.Count != numberOfWaypoints)
            {
                GenerateWaypoints();  
            }
            else if (waypoints.Count != transform.childCount)
            {
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    waypoints.Add(transform.GetChild(i).gameObject);
                }
                
            }
            else
            {
                return;
            }
        }

        private static void DestroyWaypoints(List<GameObject> destroyThisGodDamnList)
        {
            foreach (GameObject drittObjekt in destroyThisGodDamnList)
            {
                // Editor printer at jeg skal bruke DestroyImmediate()
                // mens dokumentasjonen fraråder det på det sterkeste..
                // ???
                // #YOLO
                DestroyImmediate(drittObjekt);
            }
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null)
            {
                print("No waypoints created yet!");
            }
            for (int i = 0; i < numberOfWaypoints; i++)
            {

                int j = GetNextIndex(i);

                Gizmos.DrawSphere(GetWaypoint(i), waypointSize);
                if (waypoints[i].transform.name == "Waypoint") continue;
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));

            }
        }

        private void GenerateWaypoints()
        {
            DestroyWaypoints(waypoints);
            waypoints = new List<GameObject>();
            float angleSegment = (2 * Mathf.PI) / numberOfWaypoints;
            float newAngleSegment = angleSegment*Random.value;

            for (int i = 0; i < numberOfWaypoints; i++)
            {
                if (waypoints.Count == numberOfWaypoints) return;
                // Kanskje rart at det tas kvadratrot forst ogsaa opphoye i andre.
                // Men det er for at verdien for patrolPerimeter skal bli riktig i editor. Sqrt() er for jevn fordeling.
                float patrolRadius = Mathf.Pow(Mathf.Sqrt(Random.Range(patrolPerimeter - pathFluctuation/2, patrolPerimeter + pathFluctuation/2)), 2);
                
                x_position = patrolRadius * Mathf.Sin(newAngleSegment) + transform.position.x;
                z_position = patrolRadius * Mathf.Cos(newAngleSegment) + transform.position.z;

                y_position = Terrain.activeTerrain.SampleHeight(new Vector3(x_position, 0, z_position)) + Terrain.activeTerrain.GetPosition().y;

                waypoint = new GameObject("Waypoint"+i);
                waypoint.transform.position = new Vector3(x_position, y_position, z_position);
                waypoint.transform.rotation = Quaternion.identity;
                waypoint.transform.SetParent(transform);

                //GameObject instancedWaypoint = Instantiate(waypoint, new Vector3(x_position, y_position, z_position), Quaternion.identity);
                //waypoint.transform.SetParent(transform);

                waypoints.Add(waypoint);

                newAngleSegment += angleSegment;
                
            }

        }

        private int GetNextIndex(int i)
        {
            if (i + 1 == waypoints.Count)
            {
                return 0;
            }
                
            return i + 1;
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).transform.position;
        }
    }
}
