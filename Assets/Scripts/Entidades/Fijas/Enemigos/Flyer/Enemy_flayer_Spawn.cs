using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy_flayer_Spawn : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer sr;
    public float speed;
    public List<Sprite> sprites;
    public Enemy_falyer_Manager manager;
    public GameObject particulasMuerte;

    // Start is called before the first frame update
    void Start()
    {
        manager.actual_flayer_enemies++;

        rb2d = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        direction.Normalize();
        rb2d.AddForce(direction * speed);
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[Random.Range(0, sprites.Count)];
    }

// Update is called once per frame
void Update()
{
}

public void Muerte()
{
    if (transform.localScale.x > 0.25f)
    {
        GameObject temp1 = Instantiate(manager.enemy, transform.position, Quaternion.identity);
        temp1.GetComponent<Enemy_flayer_Spawn>().manager = manager;
        temp1.transform.localScale = transform.localScale * 0.5f;

        GameObject temp2 = Instantiate(manager.enemy, transform.position, Quaternion.identity);
        temp2.GetComponent<Enemy_flayer_Spawn>().manager = manager;
        temp2.transform.localScale = transform.localScale * 0.5f;
    }
    Destroy(gameObject);
    GameObject temp = Instantiate(particulasMuerte, transform.position, transform.rotation);
    Destroy(temp, 5);
    //GameManager.instance.points += 100;
    manager.actual_flayer_enemies--;
}

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.gameObject.tag == "circulo")
    {
        collision.gameObject.GetComponent<Circulo>().Muerte();
        Destroy(gameObject);
        manager.actual_flayer_enemies--;
    }
}
}

