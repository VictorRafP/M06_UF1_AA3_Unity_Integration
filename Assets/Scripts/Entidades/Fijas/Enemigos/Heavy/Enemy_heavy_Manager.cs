using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_havy_Manager : MonoBehaviour
{
    public GameObject enemy;
    public float limit_X = 10;
    public float limit_Y = 6;
    public int max = 2;

    public int actual_heavy_enemies = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (actual_heavy_enemies <= 0)
        {
            for (int i = 0; i < max; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-limit_X, limit_X), Random.Range(-limit_Y, limit_Y));
                while (Vector3.Distance(pos, Vector3.zero) < 4f)
                {
                    pos = new Vector3(Random.Range(-limit_X, limit_X), Random.Range(-limit_Y, limit_Y));
                }
                GameObject temp = Instantiate(enemy, pos, Quaternion.identity);
                temp.GetComponent<Enemy_heavy_Spawn>().manager = this;
            }
            max++;
        }
    }
}
