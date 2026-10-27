using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ItemSpawnerr : MonoBehaviour
{
    public UsableItem[] itemPrefabs;
    public Coin coinPrefab;
    public Transform[] ItemSpawnPoints;
    public Transform[] CoinSpawnPoints;

    private readonly List<UsableItem> items = new List<UsableItem>();
    private readonly List<UsableItem> itemPool = new List<UsableItem>();
    private readonly List<Coin> coins = new List<Coin>();
    private readonly List<Coin> coinPool = new List<Coin>();
    private int wave = 0;
    public float range = 1f;

    private void Update()
    {
        if(items.Count == 0)
        {
            SpawnWave();
        }
        if(coins.Count == 0)
        {
            SpawnCoin();
        }
    }
    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave*2.5f);
        for(int i=0; i<count; i++)
        {
            CreateItem();
        }
    }
    private async Task SpawnCoin()
    {
        Coin coin;
        coin = Instantiate(coinPrefab);
        coin.gameObject.SetActive(false);
        coins.Add(coin);

        foreach(var c in coinPool)
        {
            Transform point = CoinSpawnPoints[Random.Range(0, CoinSpawnPoints.Length)];
            c.transform.position = point.position + new Vector3(Random.Range(0f, range), 0f, Random.Range(0f, range));
            c.transform.rotation = point.rotation;
            c.gameObject.SetActive(true);
            
        }
    }
    private void CreateItem()
    {

        Transform point = ItemSpawnPoints[Random.Range(0, ItemSpawnPoints.Length)];
        UsableItem itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        UsableItem item;
        if (itemPool.Count == 0)
        {
            item = Instantiate(itemPrefab);
            item.gameObject.SetActive(false);
            itemPool.Add(item);
        }

        item = itemPool[0];
        item.transform.position = point.position;
        item.transform.rotation = point.rotation;
        item.gameObject.SetActive(true);
        items.Add(item);
        itemPool.Remove(item);
        item.OnMeet.AddListener(()=>items.Remove(item));
        item.OnMeet.AddListener(()=>itemPool.Add(item));
    }
}
