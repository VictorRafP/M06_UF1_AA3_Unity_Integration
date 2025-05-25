using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Gameplay : MonoBehaviour
{
    public TMP_Text xp;
    public TMP_Text levelTime;
    //public List<Image> lifes;
    //public GameObject gameOver;
    private void Start()
    {
        //gameOver.SetActive(false);
    }
    void Update()
    {
        //xp.text = GameManager.instance.points.ToString();
        //levelTime.text = GameManager.instance.time.ToString("00.00").Replace(".", ":");
        //for (int i = 0; i < lifes.Count; i++)
        //{
        //    if (i < GameManager.instance.life)
        //    {
        //        lifes[i].color = new Color(1, 1, 1);
        //    }
        //    else
        //    {
        //        lifes[i].color = new Color(0.5f, 0, 0);
        //    }
        //}
        //if (GameManager.instance.life == 0)
        //{
        //    gameOver.SetActive(true);
        //    GameManager.instance.updateTime = false;
        //}
    }
    public void Restart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
