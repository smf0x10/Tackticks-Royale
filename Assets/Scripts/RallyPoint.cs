using UnityEngine;

/// <summary>
/// Attaches to the rally point game object to represent a rally point on the field. 
/// </summary>
public class RallyPoint : MonoBehaviour
{
    /// <summary>
    /// Total number of troops targeting this point
    /// </summary>
    private int troopsTargeting;
    

    /// <summary>
    /// Increases the number of troops targeting this point
    /// </summary>
    public void AddTroopTarget()
    {
        troopsTargeting++;
    }

    /// <summary>
    /// Decreases the number of troops targeting this rally point and destroys the point if no troops are targeting it
    /// </summary>
    public void RemoveTroopTarget()
    {
        troopsTargeting--;
        if (troopsTargeting <= 0)
        {
            Destroy(gameObject);
        }
    }
}
