using UnityEngine;

class Player : MonoBehaviour
{
    void Start()
    {
        if (tag != "Player")
        {
            tag = "Player";
        }
    }

    public void MoveTo(Vector3 pos)
    {
        Debug.Log("Player: Seems like you want me to move to " + pos +".");
    }

    public void InteractWith(GameObject obj)
    {
        Debug.Log("Player: Seems like you want me to interact with "
                  + obj.name + ".");
    }
}
