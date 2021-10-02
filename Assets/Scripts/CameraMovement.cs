using UnityEngine;

/// <summary>
/// Attatches to the camera object. Provides an interface to move the camera
/// </summary>
public class CameraMovement : MonoBehaviour {
    private readonly Vector3 p1topPos = new Vector3(34.6f, 44.4f, 0);
    private readonly Vector3 p1topRot = new Vector3(60, 270, 0);
    private readonly Vector3 p1sidePos = new Vector3(0, 26.6f, 46.7f);
    private readonly Vector3 p1sideRot = new Vector3(10, 180, 0);
    private readonly Vector3 p2topPos = new Vector3(-34.6f, 44.4f, 0);
    private readonly Vector3 p2topRot = new Vector3(60, 90, 0);
    private readonly Vector3 p2sidePos = new Vector3(0, 26.6f, -46.7f);
    private readonly Vector3 p2sideRot = new Vector3(10, 0, 0);
    private Team team;

    /// <summary>
    /// Changes the direction the camera is looking, as when the mouse is moved
    /// </summary>
    /// <param name="rotation">(Number of degrees to look right, number of degrees to look up)</param>
    public void Rotate(Vector2 rotation)
    {
        transform.RotateAround(transform.position, Vector3.up, rotation.x);
        transform.RotateAround(transform.position, transform.TransformVector(Vector3.right), -rotation.y);
        // Keep the camera from flipping upside down. I wrote this myself, but I still have no idea how quaternions work
        if (Mathf.Abs(transform.rotation.x) > Mathf.Abs(transform.rotation.w))
        {
            if ((transform.rotation.x > 0) != (transform.rotation.w > 0))
            {
                transform.rotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }
    }

    /// <summary>
    /// Moves this camera in the direction it is facing. Accounts for framerate.
    /// </summary>
    /// <param name="amount">The amount to move forward. Negative values make the camera move backwards</param>
    public void MoveForward(float amount)
    {
        transform.Translate(Vector3.forward * amount * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Sets this camera's team
    /// </summary>
    /// <param name="newTeam">The team to switch to</param>
    public void SetTeam(Team newTeam)
    {
        team = newTeam;
    }
    
    /// <summary>
    /// Moves this camera to the top view position
    /// </summary>
    /// <param name="team">The team the player is on</param>
    public void MoveToTop()
    {
        if (team == Team.Blue)
        {
            transform.position = p1topPos;
            transform.rotation = Quaternion.Euler(p1topRot);
        }
        else
        {
            transform.position = p2topPos;
            transform.rotation = Quaternion.Euler(p2topRot);
        }
    }

    /// <summary>
    /// Moves this camera to the side view position
    /// </summary>
    /// <param name="team">The team the player is on</param>
    public void MoveToSide()
    {
        if (team == Team.Blue)
        {
            transform.position = p1sidePos;
            transform.rotation = Quaternion.Euler(p1sideRot);
        }
        else
        {
            transform.position = p2sidePos;
            transform.rotation = Quaternion.Euler(p2sideRot);
        }
    }
}
