using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterController : MonoBehaviour
{
    [Header("SCREEN BUTTONS")] [SerializeField]
    private PressedButton left;

    [SerializeField] private PressedButton right;
    [SerializeField] private Button jump;
    [SerializeField] private Button fire;
    [SerializeField] private Button hit;

    [Header("TEXT ON SCREEN")] [SerializeField]
    private Text infoLabelText;

    public PressedButton Left => left;
    public PressedButton Right => right;
    public Button Jump => jump;
    public Button Fire => fire;
    public Button Hit => hit;

    void Start()
    {
        Player.Instance.InitUIController(this);
    }

    private void FixedUpdate()
    {
        MovementInputUpdate();
    }

    public void SetLabel(string label)
    {
        infoLabelText.text = "[E] " + label;
        infoLabelText.gameObject.SetActive(true);
    }

    public void CloseLabel()
    {
        infoLabelText.gameObject.SetActive(false);
    }

    private void MovementInputUpdate()
    {
        if (Player.Instance.IsDisabled) return;
        Player.Instance.Stop();
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
            Player.Instance.MovementUpdate(Vector3.left);
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
            Player.Instance.MovementUpdate(Vector3.right);
        if (Left.IsPressed)
            Player.Instance.MovementUpdate(Vector3.left);
        if (Right.IsPressed)
            Player.Instance.MovementUpdate(Vector3.right);
    }
}