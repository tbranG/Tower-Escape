using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject obstacle1;
    [SerializeField] private GameObject obstacle2;
    [SerializeField] private GameObject obstacle3;
    [SerializeField] private GameObject target;

    [SerializeField] private Vector3 mapCenter;
    [SerializeField] private float obstacleDistance;
    
    private Vector3[] obstaclePositions;

    private int currentObj1 = 0;
    private int currentObj2 = 0;
    private int currentObj3 = 0;
    private int currentTarget = 0;
    private GameObject[] obstaclePool1;
    private GameObject[] obstaclePool2;
    private GameObject[] obstaclePool3;
    private GameObject[] targetPool;

    // Start is called before the first frame update
    void Start()
    {
        //setup --------------------------------------------------------------------------+
        obstaclePositions = new Vector3[3];

        obstaclePositions[0] = new Vector3(mapCenter.x - obstacleDistance, mapCenter.y);
        obstaclePositions[1] = mapCenter;
        obstaclePositions[2] = new Vector3(mapCenter.x + obstacleDistance, mapCenter.y);

        obstaclePool1 = new GameObject[20];
        for(int i = 0; i < 20; i++)
        {
            GameObject o = Instantiate(obstacle1, Vector3.zero, Quaternion.identity);
            o.SetActive(false);

            obstaclePool1[i] = o;
        }
        obstaclePool2 = new GameObject[10];
        for(int i = 0; i < 10; i++)
        {
            GameObject o = Instantiate(obstacle2, Vector3.zero, Quaternion.identity);
            o.SetActive(false);

            obstaclePool2[i] = o;
        }
        obstaclePool3 = new GameObject[5];
        for(int i = 0; i < 5; i++)
        {
            GameObject o = Instantiate(obstacle3, Vector3.zero, Quaternion.identity);
            o.SetActive(false);

            obstaclePool3[i] = o;
        }
        targetPool = new GameObject[10];
        for(int i = 0; i < 10; i++)
        {
            GameObject o = Instantiate(target, Vector3.zero, Quaternion.identity);
            o.SetActive(false);

            targetPool[i] = o;
        }
        // --------------------------------------------------------------------------------+

        GenerateObstacle();
        StartCoroutine(SpawnOverTime());
    }

    private void GenerateObstacle()
    {
        //try generating bridge
        int chance = Random.Range(0, 10);
        if(chance == 1)
        {
            GameObject o = GetObstacleFromPool3();
            BridgeObj bridge = o.GetComponent<BridgeObj>();
            
            bridge.SetupBridge();
            o.transform.position = obstaclePositions[1];
            return;
        }

        //spike spots
        int[] selectedSpots = new int[3];

        //select positions
        for(int i = 0; i < 3; i++) selectedSpots[i] = Random.Range(0, 2);

        //prevent unpassable wall
        if (selectedSpots[0] == 1 && selectedSpots[1] == 1 && selectedSpots[2] == 1)
        {
            int s = Random.Range(0, 3);
            selectedSpots[s] = 0;
        }

        //generate obstacles


        //try generating spear
        int res = 0;
        if ((selectedSpots[0] == 1 && selectedSpots[1] == 0 && selectedSpots[2] == 0) ||
        (selectedSpots[0] == 0 && selectedSpots[1] == 0 && selectedSpots[2] == 1))
        {
            res = Random.Range(0, 2);
        }

        //res = 1, will generate a spear instead
        if (res == 1)
        {
            int pos = Random.Range(0, 2) * 2;
            GameObject o = GetObstacleFromPool2();

            o.SetActive(true);

            Vector3 fixedPos;
            if(pos == 0)
            {
                fixedPos = new Vector3(obstaclePositions[0].x - 1.4f, obstaclePositions[0].y, 0f);
                o.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                fixedPos = new Vector3(obstaclePositions[2].x + 1.4f, obstaclePositions[2].y, 0f);
                o.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            
            o.transform.position = fixedPos;
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                if (selectedSpots[i] == 1)
                {
                    GameObject o = GetObstacleFromPool1();
                    o.SetActive(true);
                    o.transform.position = obstaclePositions[i];
                }
            }
        }

        //try generating targets
        if (selectedSpots[0] == 0 && selectedSpots[1] == 0 && selectedSpots[2] == 0)
        {
            int pos = Random.Range(0, 2) * 2;
            Vector3 fixedPos;
            if(pos == 0)
            {
                fixedPos = new Vector3(obstaclePositions[0].x - 1.1f, obstaclePositions[0].y, 0f);
                GameObject o = GetTargetFromPool();
                o.SetActive(true);
                o.transform.position = fixedPos;
                o.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                fixedPos = new Vector3(obstaclePositions[2].x + 1.1f, obstaclePositions[0].y, 0f);
                GameObject o = GetTargetFromPool();
                o.SetActive(true);
                o.transform.position = fixedPos;
                o.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }

    private GameObject GetObstacleFromPool1()
    {
        GameObject obj = obstaclePool1[currentObj1];
        currentObj1++;

        if (currentObj1 >= obstaclePool1.Length) currentObj1 = 0;
        return obj;
    }

    private GameObject GetObstacleFromPool2()
    {
        GameObject obj = obstaclePool2[currentObj2];
        currentObj2++;

        if (currentObj2 >= obstaclePool2.Length) currentObj2 = 0;
        return obj;
    }

    private GameObject GetObstacleFromPool3()
    {
        GameObject obj = obstaclePool3[currentObj3];
        currentObj3++;

        if (currentObj3 >= obstaclePool3.Length) currentObj3 = 0;
        return obj;
    }

    private GameObject GetTargetFromPool()
    {
        GameObject obj = targetPool[currentTarget];
        currentTarget++;

        if (currentTarget >= targetPool.Length) currentTarget = 0;
        return obj;
    }

    private IEnumerator SpawnOverTime()
    {
        yield return new WaitForSeconds(1f);
        GenerateObstacle();
        StartCoroutine(SpawnOverTime());
    }
}
