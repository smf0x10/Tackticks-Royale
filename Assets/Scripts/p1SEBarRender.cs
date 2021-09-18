using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to display the player's summon energy bar
/// </summary>
public class p1SEBarRender : MonoBehaviour
{
    /// <summary>
    /// The number that shows the current SE value
    /// </summary>
    [SerializeField] private Text numText;
    /// <summary>
    /// The spawnManager containing the summon energy value to use
    /// </summary>
    [SerializeField] private SpawnManager seholder;
    /// <summary>
    /// The image representing the filled part of the bar
    /// </summary>
    [SerializeField] private Image thisimage;

    void Update()
    {
        if (FindObjectOfType<SpawnManager>() != null)
        {
            thisimage.fillAmount = seholder.GetSummonEnergy() * 0.05f;
            numText.text = "<b>" + Mathf.Floor(seholder.GetComponent<SpawnManager>().GetSummonEnergy()).ToString() + "</b>";
        }
    }
}
