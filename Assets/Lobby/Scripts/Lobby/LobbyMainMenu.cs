using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public InputField matchNameInput;

		public bool validateInput = true;

        public void OnEnable()
        {
            matchNameInput.onEndEdit.RemoveAllListeners();
            matchNameInput.onEndEdit.AddListener(onEndEditGameName);
        }

        public void OnClickCreateMatchmakingGame()
        {
			if (validateInput && string.IsNullOrEmpty(matchNameInput.text))
			{

				LobbyManager.s_Singleton.ShowInfoPopup("Server name cannot be empty!");
				return;
			}
			StartMatchmakingGame();
         }

		public void StartMatchmakingGame()
		{
			lobbyManager.StartMatchMaker();
			lobbyManager.matchMaker.CreateMatch(
				matchNameInput.text,
				(uint)lobbyManager.maxPlayers,
				true,
				"", "", "", 0, 0,
				lobbyManager.OnMatchCreate);

			LobbyManager.s_Singleton.ShowConnectingModal(false);
			LobbyManager.s_Singleton.state = GameState.Connecting;

		}

        void onEndEditGameName(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickCreateMatchmakingGame();
            }
        }

		public void OnBackClicked()
		{
			LobbyManager.s_Singleton.ShowDefaultPanel();
		}

	}
}
