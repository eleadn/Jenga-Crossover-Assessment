using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    [SerializeField]
    CameraController cameraController;
    [SerializeField]
    Toggle testMyStack;
    [SerializeField]
    Button stack;
    [SerializeField]
    RectTransform content;
    [SerializeField]
    float distanceBetweenButtons;

    List<Tower> towers = new List<Tower>();
    Tower focus;

    public void SetFocus(int i)
    {
        testMyStack.isOn = false;

        testMyStack.onValueChanged.RemoveAllListeners();
        focus = towers[i];
        testMyStack.onValueChanged.AddListener(focus.SwitchTestMyStack);
        Vector3 position = focus.transform.position;
        position.y = focus.GetHalfHeight();
        cameraController.SetFocus(position);
    }
    
    void AddButton(string name, int index)
    {
        Button button = Instantiate(stack);
        RectTransform buttonTransform = button.GetComponent<RectTransform>();
        buttonTransform.SetParent(content, false);
        button.GetComponentInChildren<TextMeshProUGUI>().text = name;
        Vector3 newPosition = buttonTransform.localPosition;
        newPosition.y -= index * (buttonTransform.rect.height + distanceBetweenButtons);
        buttonTransform.localPosition = newPosition;
        button.GetComponent<TowerButton>().Index = index;

        button.onClick.AddListener(() => 
        {
            SetFocus(button.GetComponent<TowerButton>().Index);
        });
    }

    public void AddTower(Tower tower)
    {
        towers.Add(tower);
        AddButton(tower.Grade, towers.Count - 1);

        if (towers.Count == 1)
            SetFocus(0);
    }
}
