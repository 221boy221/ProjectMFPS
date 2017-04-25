using ExitGames.Client.Photon.Chat;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChannelSelector : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _channelName;

    public void SetHighlight(bool on)
    {
        if (on)
        {
            this.GetComponent<Image>().color = Color.green;
        }
        else
        {
            this.GetComponent<Image>().color = Color.red;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ChatBox.Instance.JoinChannel(_channelName);
    }
}
