using UnityEngine;

public class BattleHUDFollow : MonoBehaviour
{
    public Rigidbody2D character;
    public Transform HUDTransform;

    void Start()
    {
        HUDTransform.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 2f, 0f);
    }

    void FixedUpdate()
    {
        HUDTransform.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 2f, 0f);
    }
}
