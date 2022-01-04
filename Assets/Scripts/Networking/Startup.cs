using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;

public class Startup : MonoBehaviour
{
    [SerializeField] private Button host;
    [SerializeField] private Button join;
    [SerializeField] private NetworkManager netMan;
    private void Start()
    {
        host.onClick.AddListener(BeginHost);
        join.onClick.AddListener(BeginClient);
    }

    private void SwitchToOnline()
    {
        SceneManager.LoadScene("Online");
    }

    public void BeginHost()
    {
        if (!netMan.StartHost())
        {
            return;
        }
        SwitchToOnline();
    }

    public void BeginClient()
    {
        if (!netMan.StartClient())
        {
            return;
        }
        SwitchToOnline();
    }
}
