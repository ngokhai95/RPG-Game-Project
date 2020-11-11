using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject prefab;
    public int numObjects;
    public float spawnSize;
    private LayerMask layerMask = ~0;
    private int seedNo = 12345;
    private Vector3 offset;


    private float checkingHeight = 100000f;
    private Quaternion rotation;
    
    [HideInInspector] public GameObject[] spawnObjs;
    private GameObject spawnObj;
    private bool updateSettings = false;

    private void OnValidate()
    {
        if (Time.realtimeSinceStartup > 10f)
            updateSettings = true;

#if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
#endif
    }

    private void OnEnable()
    {
        if (Time.realtimeSinceStartup > 10f)
            InitObjects();
    }

    private void OnDisable()
    {
        RemoveObjects();
    }

    private void CreateParent()
    {
        if (GameObject.Find("Object Spawn") == null)
            spawnObj = new GameObject("Object Spawn");
        else
            spawnObj = GameObject.Find("Object Spawn");
    }

    private void RemoveParent()
    {
        if (GameObject.Find("Object Spawn") != null)
            DestroyImmediate(GameObject.Find("Object Spawn"));
    }

    private void RemoveObject(int i)
    {
        if (Application.isPlaying)
            Destroy(spawnObjs[i]);
        else
            DestroyImmediate(spawnObjs[i]);

        spawnObjs.ToList().RemoveAt(i);
    }

    private void RemoveObjects()
    {
        if (spawnObjs != null && spawnObjs.Length > 0)
            for (int i = 0; i < spawnObjs.Length; i++)
                RemoveObject(i);
    }

    private void Awake()
    {
        if (Application.isPlaying)
            RemoveParent();
    }

    private void Start()
    {
        if (Application.isPlaying)
            InitObjects();
    }

    void Update()
    {
        if (player == null || prefab == null) return;
        UpdateObjects();
    }

    void InitObjects()
    {
        spawnObjs = new GameObject[numObjects];
        if (player == null || prefab == null) return;
        CreateParent();
        CreateObjects();
    }

    private void CreateObjects()
    {
        for (int i = 0; i < numObjects; i++)
        {
            spawnObjs[i] = Instantiate(prefab, spawnObj.transform);
            spawnObjs[i].name = prefab.name + "_" + (i + 1).ToString();
        }
    }

    private void Spawn(int i, Vector3 spawnLocation)
    {
        if (player == null || prefab == null) return;

        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        spawnLocation.y = checkingHeight;
        Ray ray = new Ray(spawnLocation, Vector3.down);

        if (!Raycasts.RaycastNonAllocSorted(ray, false, false, layerMask))
            return;

        RaycastHit hit = Raycasts.closestHit;
        spawnLocation = hit.point;

        //position
        spawnObjs[i].transform.position = spawnLocation + offset;

        // rotation
        rotation = Quaternion.Euler(new Vector3(Random.Range(-5, 5), Random.Range(0, 360), Random.Range(-5, 5)));
        spawnObjs[i].transform.rotation = rotation;

        // scale
        Vector3 lossyScale = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
        
        spawnObjs[i].transform.localScale = lossyScale;
        


        Physics.autoSimulation = true;
    }
    private Vector3 RandomSpawnLocation(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    private void UpdateObjects()
    {
        if (!Application.isPlaying && updateSettings)
        {
            InitObjects();
            updateSettings = false;
        }

        for (int i = 0; i < spawnObjs.Length; i++)
        {
            if (spawnObjs[i] == null) return;

            float distance = (new Vector2(spawnObjs[i].transform.position.x, spawnObjs[i].transform.position.z) - new Vector2(player.transform.position.x, player.transform.position.z)).sqrMagnitude;

            if (distance > spawnSize * spawnSize)
            {
                spawnObjs[i].SetActive(false);

                Spawn(i, RandomSpawnLocation(player.transform.position, spawnSize * 0.85f));
            }
            else
                spawnObjs[i].SetActive(true);
        }
    }

}