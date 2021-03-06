using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    [HideInInspector]
    public SpriteRenderer sprite;

    void Start() 
    {
        sprite = this.GetComponent<SpriteRenderer>();
        sprite.sprite = item.icon;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Players")) {
            Debug.Log("Picking up " + item.name);
            Inventory.instance.AddItem(item);
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Pickup Item");
            Destroy(gameObject);
        }
    }
}
