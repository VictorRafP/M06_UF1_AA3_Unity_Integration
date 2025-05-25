using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string tutorialEnemyTag = "TutorialEnemy";
    public string eventName = "EnemyReachedCircle";


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tutorialEnemyTag))
        {
            EventManager.TriggerEvent(eventName);
            gameObject.SetActive(false);
        }
    }
}
