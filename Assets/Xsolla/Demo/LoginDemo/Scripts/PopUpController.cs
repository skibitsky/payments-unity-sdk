using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class PopUpController : MonoBehaviour, IPopUpController
{
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject extendedPanel;

    public UnityAction OnClosePopUp
    {
        set
        {
            successPanel.GetComponent<IPopUp>().OnClose = value;
            errorPanel.GetComponent<IPopUp>().OnClose = value;
            extendedPanel.GetComponent<IPopUp>().OnClose = value;
        }
    }
    public UnityAction OnReturnToLogin
    {
        set => extendedPanel.GetComponent<IExtendedPopUp>().OnReturnToLogin = value;
    }
    public void ShowPopUp(string message, PopUpWindows popUp)
    {
        switch (popUp)
        {
            case PopUpWindows.Success:
                successPanel.GetComponent<IPopUp>().ShowPopUp(message);
                break;

            case PopUpWindows.Error:
                errorPanel.GetComponent<IPopUp>().ShowPopUp(message);
                break;

            case PopUpWindows.Warning:
                warningPanel.GetComponent<IPopUp>().ShowPopUp(message);
                break;
            case PopUpWindows.Extended:
                extendedPanel.GetComponent<IPopUp>().ShowPopUp(message);
                break;
        }
    }
    public void ShowPopUp(string header, string message)
    {
        extendedPanel.GetComponent<IExtendedPopUp>().ShowPopUp(header, message);
    }
}
