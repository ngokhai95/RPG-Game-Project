using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class GameObject_Spawner : MonoBehaviour
{
    public GameObject player;
    public GameObject prefab_obj;
    public GameObject prefab_enemy;
    GameObject prefab;
    public int numObjects = 10;
    public float spawnSize = 100f;
    public LayerMask layerMask = ~0;
    public int seedNo = 12345;
    public Vector3 offset;


    private float checkingHeight = 100000f;
    private Quaternion rotation;
    private float ratio = 0.2f;
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
        if (GameObject.Find("GameObject Spawner") == null)
            spawnObj = new GameObject("GameObject Spawner");
        else
            spawnObj = GameObject.Find("GameObject Spawner");
    }

    private void RemoveParent()
    {
        if (GameObject.Find("GameObject Spawner") != null)
            DestroyImmediate(GameObject.Find("GameObject Spawner"));
    }

    private void RemoveObjects()
    {
        if (spawnObjs != null && spawnObjs.Length > 0)
            for (int i = 0; i < spawnObjs.Length; i++)
                DestroyImmediate(spawnObjs[i]);
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
        if (prefab_enemy != null)
        {
            prefab = prefab_enemy;
            //RemoveObjects();
            CreateParent();
            CreateEnemies();
        }
        else
        {
            //RemoveObjects();
            CreateParent();
        }
        prefab = prefab_obj;
        if (player == null || prefab == null) return;      
        CreateObjects();
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < Mathf.FloorToInt(numObjects * ratio); i++)
        {
            spawnObjs[i] = Instantiate(prefab, spawnObj.transform);
            spawnObjs[i].name = prefab.name + "_" + (i + 1).ToString();
        }    
    }

    private void CreateObjects()
    {
        if (prefab_enemy != null)
        {
            for (int i = Mathf.FloorToInt(numObjects * ratio); i < numObjects; i++)
            {
                spawnObjs[i] = Instantiate(prefab, spawnObj.transform);
                spawnObjs[i].name = prefab.name + "_" + (i + 1).ToString();
            }
        }
        else
        {
            for (int i = 0; i < numObjects; i++)
            {
                spawnObjs[i] = Instantiate(prefab, spawnObj.transform);
                spawnObjs[i].name = prefab.name + "_" + (i + 1).ToString();
            }
        }
    }

    private void Spawn(int i, Vector3 origin)
    {
        if (player == null || prefab == null) return;

        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        origin.y = checkingHeight;
        Ray ray = new Ray(origin, Vector3.down);

        if (!Raycasts.RaycastNonAllocSorted(ray, false, false, layerMask))
            return;

        RaycastHit hit = Raycasts.closestHit;
        Vector3 normal = hit.normal;
        origin = hit.point;

        //position
        spawnObjs[i].transform.position = origin + offset;
        // rotation
        if (prefab_enemy == null)
        {
            rotation = Quaternion.Euler(new Vector3(Random.Range(-5, 5), Random.Range(0, 360), Random.Range(-5, 5)));
            spawnObjs[i].transform.rotation = rotation;
        }
        else
        {
            rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            spawnObjs[i].transform.rotation = rotation;
        }


        // scale
        if (prefab_enemy == null)
        {
            Vector3 lossyScale = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
            spawnObjs[i].transform.localScale = lossyScale;
        }
        else
        {
            Vector3 lossyScale = new Vector3(4f, 4f, 4f);
            spawnObjs[i].transform.localScale = lossyScale;
        }
       

        Physics.autoSimulation = true;
    }

    private void RemoveObject(int i)
    {
        if (Application.isPlaying)
            Destroy(spawnObjs[i]);
        else
            DestroyImmediate(spawnObjs[i]);

        spawnObjs.ToList().RemoveAt(i);
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

                Spawn(i, DrawCircle(player.transform.position, spawnSize * 0.85f));
            }
            else
                spawnObjs[i].SetActive(true);
        }
    }

    private Vector3 DrawCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
