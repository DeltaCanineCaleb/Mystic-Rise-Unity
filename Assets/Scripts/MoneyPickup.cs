using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    public Sprite icon;
    [HideInInspector]
    public SpriteRenderer sprite;
    public int amount;

    void Start() 
    {
        sprite = this.GetComponent<SpriteRenderer>();
        sprite.sprite = icon;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Players")) {
            Debug.Log("Picking up $" + amount);
            Inventory.arcs += amount;
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Pickup Item");
            Destroy(gameObject);
        }
    }
}
