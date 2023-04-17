using UnityEngine;
using Unity.Services.Relay;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;

public class Matchmaking : MonoBehaviour
{
    private UnityTransport transport;

    private const int MaxPlayers = 2;
    private static bool authenticated = false;

    private async void Awake()
    {
        transport = FindObjectOfType<UnityTransport>();
        await Authenticate();
    }

    private static async Task Authenticate()
    {
        if (!authenticated)
        {
            authenticated = true;
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void HostGame()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);
        UI.code.text = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();
    }

    public async void JoinGame(string joinInput)
    {
        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinInput);

        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }

    public void ExitMatch()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
