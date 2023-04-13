using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private bool serverAuth;

    private NetworkVariable<NetworkState> netState;

    private void Awake()
    {
        var permission = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        netState = new NetworkVariable<NetworkState>(writePerm: permission);
    }

    private void Update()
    {
        if (IsOwner)
            TransmitState();
        else
            ReadState();
    }

    private void TransmitState()
    {
        var state = new NetworkState
        {
            Position = transform.position,
        };

        if (IsServer || !serverAuth)
            netState.Value = state;
        else
            TransmitStateServerRpc(state);
    }

    [ServerRpc]
    private void TransmitStateServerRpc(NetworkState state)
    {
        netState.Value = state;
    }

    private void ReadState()
    {
        transform.position = netState.Value.Position;
    }

    private struct NetworkState : INetworkSerializable
    {
        private float pX, pZ;


        internal Vector3 Position
        {
            get => new (pX, 3, pZ);
            set
            {
                pX = value.x;
                pZ = value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref pX);
            serializer.SerializeValue(ref pZ);
        }
    }
}