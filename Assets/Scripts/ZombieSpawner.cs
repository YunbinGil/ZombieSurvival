using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class ZombieSpawner : MonoBehaviour
{
    public UIManager uiManager;
    public Zombie zombiePrefab;
    public ZombieData[] zombieDatas;
    public Transform[] spawnPoints;

    private readonly List<Zombie> zombies = new List<Zombie>();
    private readonly List<Zombie> zombiePool = new List<Zombie>(); //비활성화 된 zombies

    private int wave = 0;

    private void Update()
    {
        if(zombies.Count == 0)
        {
            SpawnWave();

        }
    }
    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave*1.5f);
        for(int i=0; i<count; i++)
        {
            CreateZombie();
        }

        uiManager.SetWaveInfoText(wave, zombies.Count);
    }
    private void CreateZombie()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];

        //[생성]첫번째 인자에 Zombie형을 줬으니 Instantiate는 생성된 GO의 Zombie component를 리턴
        // Zombie zombie = Instantiate(zombiePrefab, point.position, point.rotation);
        
        Zombie zombie;
        if(zombiePool.Count == 0)
        {
            zombie = Instantiate(zombiePrefab);
            zombie.gameObject.SetActive(false);
            zombiePool.Add(zombie);
            zombie.OnDead.AddListener(()=>zombies.Remove(zombie)); //OnDead에 메소드를 등록하는. 매개변수x, 반환x 메소드 이름 
        
            // zombie.OnDead.AddListener(()=>zombiePool.Add(zombie)); //x. 이렇게 하면 죽음처리 되기전에 바로 사용됨.
            zombie.OnDead.AddListener(()=>StartCoroutine(CoReturnToPoolZombie(zombie)));
            //[삭제]
            //zombie.OnDead.AddListener(()=>Destroy(zombie.gameObject, 5f)); //Destory 2번째인자: 지연. 5초후에 destory 
            zombie.OnDead.AddListener(()=>uiManager.SetWaveInfoText(wave, zombies.Count));
        }
        
        zombie = zombiePool[0]; //생성이 아닌 재사용 
    
        zombie.Setup(data);
        zombie.transform.position = point.position; //생성이 아닌 활성화 될때 pos, rot 초기화
        zombie.transform.rotation = point.rotation;
        zombie.gameObject.SetActive(true);
        zombies.Add(zombie); //zombies.Remove(zombie) //zombie는 참조형이라 ㅇㅇ
        zombiePool.Remove(zombie);
        
        
        
    }

    private IEnumerator CoReturnToPoolZombie(Zombie zombie)
    {
        yield return new WaitForSeconds(5f); //5초 후에
        zombie.gameObject.SetActive(false);
        zombiePool.Add(zombie);
    }
}
