using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralEvent : MonoBehaviour
{
    public float randomDistance;
    public float minimumDistance;
    public float packSpread;
    public float Angle;
    public float height;
    public Vector3 distance;
    public GameObject coob;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    //NEXT THING TO WORK ON
    public void SpawnWave(){
        for (int i = 0; i < 1; i += 1){
            RaycastHit hit;
            Angle = 360 * Random.value;
            distance = new Vector3(randomDistance * Random.value + minimumDistance, height, 0);
            distance = Quaternion.Euler(0, Angle, 0) * distance;
            distance += new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Debug.DrawLine(transform.position, distance, Color.red, 5f);
            Debug.DrawLine(distance, new Vector3(distance.x, distance.y - height * 2, distance.z), Color.red, 5f);
            if (Physics.Raycast(distance, new Vector3(0, 0 - height * 2, 0), out hit, height * 2)){
                for (int e = 0; e < 4; e += 1){
                    RaycastHit spawn;
                    
                    Angle = 360 * Random.value;
                    distance = new Vector3(packSpread * Random.value, height, 0);
                    
                    distance = Quaternion.Euler(0, Angle, 0) * distance;
                    distance += new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    Debug.DrawLine(hit.point, distance, Color.blue, 20f);
                    Debug.DrawLine(distance, new Vector3(distance.x, distance.y - height * 2, distance.z), Color.blue, 20f);
                    if (Physics.Raycast(distance, new Vector3(0, 0 - height * 2, 0), out spawn, height * 2)){
                        Instantiate(coob, spawn.point, Quaternion.Euler(spawn.normal.x, spawn.normal.y, spawn.normal.z));
                        Debug.Log("spawned object");
                    }
                }
                //Instantiate(coob, hit.point, Quaternion.Euler(hit.normal.x, hit.normal.y, hit.normal.z));
                Debug.Log("spawned object");
            }

        }
        
        /*for (int i = 0; i < 3; i += 1){
            RaycastHit hit;
            
            Angle = 360 * Random.value;
            distance = new Vector3(transform.position.x + randomDistance * Random.value + minimumDistance, transform.position.y + height, transform.position.z);
            
            distance = Quaternion.Euler(0, Angle, 0) * distance;
            distance += new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Debug.DrawLine(transform.position, distance, Color.red, 5f);
            Debug.DrawLine(distance, new Vector3(distance.x, distance.y - height * 2, distance.z), Color.red, 5f);
            if (Physics.Raycast(distance, new Vector3(0, 0 - height * 2, 0), out hit, height * 2)){
                for (int e = 0; e < 100; e += 1){
                    RaycastHit spawn;
                    
                    Angle = 360 * Random.value;
                    distance = new Vector3(hit.point.x + packSpread * Random.value, hit.point.y + height, hit.point.z);
                    
                    distance = Quaternion.Euler(0, Angle, 0) * distance;
                    distance += new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    Debug.DrawLine(hit.point, distance, Color.blue, 20f);
                    Debug.DrawLine(distance, new Vector3(distance.x, distance.y - height * 2, distance.z), Color.blue, 20f);
                    if (Physics.Raycast(distance, new Vector3(0, 0 - height * 2, 0), out spawn, height * 2)){
                        //Instantiate(coob, spawn.point, Quaternion.Euler(spawn.normal.x, spawn.normal.y, spawn.normal.z));
                        Debug.Log("spawned object");
                    }
                }
                //Instantiate(coob, hit.point, Quaternion.Euler(hit.normal.x, hit.normal.y, hit.normal.z));
                Debug.Log("spawned object");
            }
        }*/
    }
}
