using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    /// <summary>
    /// Destroys the gameobject this script is attatched to
    /// </summary>
    public void Kill()
    {
        Destroy(gameObject);
    }
}
