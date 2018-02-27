using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class Launcher : Photon.PunBehaviour
    {
        #region Public Variables
        public GameObject playerPrefab;
        /// <summary>
        /// The PUN loglevel. 
        /// </summary>
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
        public int prePlayersNumber = 0;
        public string prePlayerName = "";

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>   
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        public byte MaxPlayersPerRoom = 4;
        #endregion


        #region Private Variables


        /// <summary>
        /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
        /// </summary>
        string _gameVersion = "1";


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {

            // #NotImportant
            // Force LogLevel
            PhotonNetwork.logLevel = Loglevel;
            // #Critical
            // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
            PhotonNetwork.autoJoinLobby = false;


            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.playerName = "VRClay";
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            Connect();
        }

        void Update()
        {
            if (PhotonNetwork.playerList.Length > 1 )
            {   
                for (int i=0; i<PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].name != prePlayerName) {
                        Debug.Log(PhotonNetwork.playerList[i].name + " just joined the game.");
                    }
                }
            }
        }
        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process. 
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {

            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.connected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
            }
        }


        #endregion

        #region Photon.PunBehaviour CallBacks


        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRandomRoom();
        }


        public override void OnDisconnectedFromPhoton()
        {
        }

        #endregion
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log(PhotonNetwork.playerName + " create the game.");
            prePlayerName = PhotonNetwork.playerName;
           GameObject go = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
            if (go.GetComponent<PhotonView>().isMine)
                go.GetComponent<Renderer>().material.color = Color.red;
            else
                Debug.Log("XXXXXXXXXXXXXXXllo");
        }
    }
}