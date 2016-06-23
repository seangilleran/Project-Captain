using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The SceneManager should be keeping track of all Actors in play at 
/// any given time in order to figure out which is selected. It should 
/// also be responsible for conveying messages and state information 
/// from Actor to Actor.
/// </summary>
class SceneManager : MonoBehaviour
{
    List<GameObject> Actors;
    GameObject Player;
    GameObject SelectedActor;

    /// <summary>
    /// Look for Actors and add them to the list. Find the actor 
    /// representing the player and store that separately.
    /// </summary>
    void Start()
    {
        Actors = new List<GameObject>();
        Player = null;
        SelectedActor = null;

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.GetComponent<Actor>())
            {
                if (obj.tag == "Player")
                {
                    if (Player != null)
                    {
                        throw new System.Exception(
                            "SceneManager: Too many player objects in Scene.");
                    }
                    Player = obj;
                    continue;
                }
                Actors.Add(obj);
            }
        }
        if (Actors.Count == 0)
        {
            throw new System.Exception("SceneManager: No Actors in Scene.");
        }
        if (Player == null)
        {
            throw new System.Exception("SceneManager: No player in Scene.");
        }
    }

    void Update()
    {
        if (EventSystem.current != null
            && EventSystem.current.IsPointerOverGameObject())
        {
            // Don't look for mouse input if the pointer is over a UI
            // object--the EventSystem will handle things from there.
            return;
        }
        if (Input.GetButtonUp("Action"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                var obj = hit.transform.gameObject;
                if (Actors.Contains(obj))
                {
                    Player.SendMessage("InteractWith", obj);
                }
                return;
            }
            Player.SendMessage("MoveTo",
                Camera.main.ScreenToWorldPoint(Input.mousePosition));
            return;
        }
        if (Input.GetButtonUp("Select"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                var obj = hit.transform.gameObject;
                if (Actors.Contains(obj))
                {
                    SelectedActor = this.SelectObject(obj);
                }
            }
            return;
        }
    }

    /// <summary>
    /// Test method for use with UI controls.
    /// </summary>
    public void DebugPing()
    {
        Debug.Log("Hello!");
    }

    /// <summary>
    /// Select an object in the scene. This will tell the object that
    /// it is selected and deselect other objects. The object must 
    /// handle its own logic after that.
    /// </summary>
    /// <param name="obj">Object to be selected</param>
    GameObject SelectObject(GameObject obj)
    {
        if (!Actors.Contains(obj))
        {
            Debug.Log("SceneManager: Object is not an Actor and cannot be "
                      + "selected.");
            return null;
        }

        foreach (var actor in Actors)
        {
            actor.SendMessage("Deselect");
        }
        obj.SendMessage("Select");
        return obj;
    }
}
