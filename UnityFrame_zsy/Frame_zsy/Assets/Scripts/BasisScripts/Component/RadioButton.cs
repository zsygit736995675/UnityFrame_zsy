using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

 //[AddComponentMenu("UI/RadioButton", 30)]
public class RadioButton : Selectable
{
    private static Dictionary<string, List<RadioButton>> checkButtons = new Dictionary<string, List<RadioButton>>();
    public string id;
    [SerializeField]
    public string group;

    [SerializeField]
    private GameObject backgroundOn;
    [SerializeField]
    private GameObject backgroundOff;

    [SerializeField]
    private bool _isChecked = false;
    public bool isChecked
    {
        get
        {
            return _isChecked;
        }
        set
        {
            _isChecked = value;
            backgroundOn.SetActive(_isChecked);
            backgroundOff.SetActive(!_isChecked);
        }
    }
    protected override void  Start() {

        List<RadioButton> Buttonlist = null;
        if (!checkButtons.TryGetValue(group, out Buttonlist))
        {
            Buttonlist = new List<RadioButton>();
            checkButtons.Add(group, Buttonlist);
        }
        Buttonlist.Add(this);
        isChecked = _isChecked;
    }
    protected override void OnDestroy()
    {
        List<RadioButton> buttons;
        if (checkButtons.TryGetValue(group, out buttons))
        {
            buttons.Remove(this);
        }
    }
    private bool MayDrag(PointerEventData eventData)
    {
        return IsActive() &&
               IsInteractable() &&
               eventData.button == PointerEventData.InputButton.Left;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!MayDrag(eventData))
            return;
        EventSystem.current.SetSelectedGameObject(gameObject, eventData);
        List<RadioButton> buttons;
        if (checkButtons.TryGetValue(group, out buttons))
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                RadioButton button = buttons[i];
                button.isChecked = button == this;
            }
        }
        base.OnPointerUp(eventData);
        eventData.Use();
    }
}
