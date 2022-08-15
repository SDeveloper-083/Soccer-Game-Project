using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    PlayerController playerController;
    public Image prefabCursor;
    public Text name;
    public Transform UITarget;
    private Image UIUse;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        UIUse = prefabCursor.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UIUse.transform.position = Camera.main.WorldToScreenPoint(UITarget.position);
    }
    private void LateUpdate()
    {
        name.text = playerController.playerManager.playerData.PlayerName;
    }
}
