using UnityEngine;

public class PointsZone2D : MonoBehaviour
{
    [SerializeField] private int pointsAmount = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        ///// Check if the object hitting this is the player
        ///// if the player enters this trigger zone, it will add points to the player's
        ///// score and then destroy this game object so it can't be collected again 
        if (other.CompareTag("Player"))
        {
            ///// add points to the player's score using the GameManager's AddPoint function, passing the pointsAmount as a parameter
            GameManager.Instance.AddPoint(pointsAmount);
            Destroy(gameObject);
        }
    }
}
