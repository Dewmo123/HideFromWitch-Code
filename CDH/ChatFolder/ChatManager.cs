using AKH.Network;
using Assets._00.Work.CDH.Code.ChatFolder;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.Utiles;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _00.Work.CDH.Code.ChatFolder
{
    public class ChatManager : MonoBehaviour
    {
        [Header("Chat Manager")]
        [SerializeField] private EventChannelSO packetEventChannel;

        // [SerializeField] private ChatGenerator chatGenerator;
        [SerializeField] private RectTransform chatsTrm;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Chat chatPrefab;

        private List<Chat> _chats;
        private float maxSize = 0f;
        private ChatGenerator _chatGenerator;

        [Header("Chat Input")]
        [SerializeField] private EventChannelSO chatEventChannel;
        [SerializeField] private TMP_InputField chatInputField;
        [SerializeField] private GameObject chatUIObj;
        [SerializeField] private InputSO playerInput;
        private bool _isChat = false;
        private void Awake()
        {
            _chats = new List<Chat>();
            packetEventChannel.AddListener<ChatRecvEventHandler>(RecvChat);
            _chatGenerator = new ChatGenerator();
            chatInputField.onSelect.AddListener(HandleSelect);
            chatInputField.onDeselect.AddListener(HandleDeSelect);
            playerInput.OnEnterEvent += HandleEnter;
            playerInput.OnEscapeEvent += HandleEscape;
            _chatGenerator.Initialize(chatPrefab);
            chatInputField.interactable = false;
        }
        private void OnDestroy()
        {

        }
        private void HandleEscape()
        {
            if (_isChat)
            {
                chatInputField.interactable = false;
            }
        }


        private async void HandleEnter()
        {
            if (!_isChat)
            {
                chatInputField.interactable = true;
                chatInputField.Select();
                await Awaitable.NextFrameAsync();
                chatInputField.ActivateInputField();
            }
            else
            {
                HandleSubmit(chatInputField.text);
            }
        }

        private void HandleSubmit(string arg0)
        {
            _isChat = false;
            chatInputField.interactable = false;
            if (!CheckChatText(arg0))
            {
                Debug.Log("�޽����� ���� ä���� ������ �ʽ��ϴ�.");
                return;
            }
            SendChat(arg0);
            chatInputField.text = "";
        }

        private void HandleDeSelect(string arg0)
        {
            _isChat = false;
            playerInput.SetEnable(true);
        }

        private void HandleSelect(string arg0)
        {
            _isChat = true;
            playerInput.SetEnable(false);
        }

        private bool CheckChatText(string message)
        {
            if (message == "" || message.Length >= 30)
                return false;
            return true;
        }
        private void RecvChat(ChatRecvEventHandler evt)
        {
            print("���� ê ��Ŷ : " + evt.pName + " : " + evt.message);

            Chat newChat = _chatGenerator.Generate(evt.pName, evt.message);
            newChat.transform.SetParent(chatsTrm);
            newChat.transform.localScale = Vector3.one;
            newChat.transform.rotation = chatsTrm.rotation;
            newChat.transform.localPosition = Vector3.zero;
            if (maxSize < newChat.transform.position.x)
                maxSize = newChat.transform.position.x;

            _chats.Add(newChat);

            Canvas.ForceUpdateCanvases();
            chatsTrm.anchoredPosition = new Vector3(maxSize, 0, 0);
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void SendChat(string message)
        {
            C_Chat newChat = new C_Chat();
            newChat.text = message;
            NetworkManager.Instance.SendPacket(newChat);

            print("���� ê ��Ŷ : " + newChat.text);
        }
    }
}