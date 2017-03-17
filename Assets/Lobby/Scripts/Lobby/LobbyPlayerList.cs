using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
    //List of players in the lobby
    public class LobbyPlayerList : MonoBehaviour
    {
		public LobbyManager lobbyManager;
		public LobbyInfoPanel infoPanel;
        public static LobbyPlayerList _instance = null;

        public RectTransform playerListContentTransform;

        protected VerticalLayoutGroup _layout;
        public List<LobbyPlayer> _players = new List<LobbyPlayer>();

        public void OnEnable()
        {
            _instance = this;
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
			if (lobbyManager != null)
			{
				lobbyManager.clientDisconnected += OnDisconnect;
			}
        }

		void OnDisable()
		{
			if (lobbyManager != null)
			{
				lobbyManager.clientDisconnected -= OnDisconnect;
			}
		}

        void Update()
        {
            //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
            //sometime to child being assigned before layout was enabled/init, leading to broken layouting)
            
            if(_layout)
                _layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

            player.transform.SetParent(playerListContentTransform, false);

            PlayerListModified();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            _players.Remove(player);
            PlayerListModified();
        }

        public void PlayerListModified()
        {
            int i = 0;
            foreach (LobbyPlayer p in _players)
            {
                p.OnPlayerListChanged(i);
                p.SetPlayerId(i);
                ++i;
            }
        }

		protected virtual void OnDisconnect(UnityEngine.Networking.NetworkConnection conn)
		{
			if (lobbyManager != null)
			{
				lobbyManager.ShowDefaultPanel();
				infoPanel.Display("Disconnected from server", "Cancel", null);

				lobbyManager.Disconnect();
			}
		}
    }
}
