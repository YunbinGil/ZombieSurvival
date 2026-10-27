using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    private List<GameObject>[] itemPool;
    private float timer;
    public float spawnInterval = 3f;

    private void Awake()
    {
        itemPool = new List<GameObject>[itemPrefabs.Length];
        for(int i=0; i<itemPool.Length; i++)
        {
            itemPool[i] = new List<GameObject>();
        }
    }
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
        int index = Random.Range(0, itemPrefabs.Length);

        if (itemPool[index].Count == 0)
        {
            GameObject newItemGo = Instantiate(itemPrefabs[index]);
            itemPool[index].Add(newItemGo);
            newItemGo.SetActive(false);

            IItem newItem = newItemGo.GetComponent<IItem>();
            if(newItem != null)
            {
                newItem.OnUse.AddListener(()=> itemPool[index].Add(newItemGo));
            }
        }
        Vector3 pos = Vector3.zero;
        
        pos.x = Random.Range(minX, maxX);
        pos.z = Random.Range(minZ, maxZ);

        if(NavMesh.SamplePosition(pos, out NavMeshHit hit, 10f, NavMesh.AllAreas)){
            pos = hit.position;
            pos.y = 0.5f;
            
            itemPool[index][0].transform.position = pos;
            itemPool[index][0].SetActive(true);
            itemPool[index].RemoveAt(0);
        }
        else
        {
            Debug.Log("실패");
        }

    }
}
