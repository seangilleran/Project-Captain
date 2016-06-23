using UnityEngine;

/// <summary>
/// This component is used by the SceneManager to determine which 
/// objects are selected and which are not.
/// </summary>
class Actor : MonoBehaviour
{
    bool Selected;

    void Start()
    {
        Selected = false;
    }

    public void Select()
    {
        Selected = true;
        Debug.Log(this.name + ": I've been selected!");
    }

    public void Deselect()
    {
        Selected = false;
    }
}