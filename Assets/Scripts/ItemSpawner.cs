using System.Collections.Generic;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    private readonly List<GameObject> items = new List<GameObject>();
    private readonly List<GameObject> itemPool = new List<GameObject>();
    private float timer;
    public float spawnInterval = 3f;


    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawnInterval)
        {
            timer=0;
            CreateItem();
        }
    }
    
    private void CreateItem()
    {
        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        Vector3 pos = Vector3.zero;
        
        pos.x = Random.Range(minX, maxX);
        pos.z = Random.Range(minZ, maxZ);

        if(NavMesh.SamplePosition(pos, out NavMeshHit hit, 10f, NavMesh.AllAreas)){
            pos = hit.position;
            pos.y = 0.5f;
            Instantiate(prefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("실패");
        }

    }
}
