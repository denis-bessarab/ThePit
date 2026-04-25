using UnityEngine;

public class Script_1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Character")
        {
            InstantiateStaminaResetThing();

            Destroy(gameObject);
        }
    }

    private void InstantiateStaminaResetThing()
    {
        var go = Resources.Load("Prefabs/StaminaResetThing") as GameObject;
        var srt = Instantiate(go);
        srt.transform.position = new Vector3(116f, -65f, 0);
    }
}
