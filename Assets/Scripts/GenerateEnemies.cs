using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject theEnemy;
    public float xPos;
    public float zPos;
    public int enemyCount;
    float timer;
    // Start is called before the first frame update
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= .6f)
        {
            StartCoroutine(EnemyDrop());
            timer = 0;
        }
    }


    IEnumerator EnemyDrop() 
    { 
        xPos = Random.Range(0, 90);
        zPos = Random.Range(-70, 0);    
        Instantiate(theEnemy, new Vector3(xPos, 5, zPos), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
    }
}
