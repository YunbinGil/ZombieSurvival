using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ItemSpawner : MonoBehaviour
{
    public UsableItem[] itemPrefabs;
    public Coin coinPrefab;
    private int wave = 0;

    private void Update()
    {
        if(wave == 0)
        {
            SpawnWave();
        }
    }
    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave*2.5f);
        for(int i=0; i<10; i++)
        {
            CreateItem();
        }
    }
    
    private void CreateItem()
    {
        UsableItem item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
        item.transform.position = new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f));
        item.gameObject.SetActive(true);
    }
}
