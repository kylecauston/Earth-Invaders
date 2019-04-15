using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Color allyColor = Color.blue;
    public Color enemyColor = Color.red;
    public Color neutralColor = Color.gray;
    public Material ground = null;

    public GameObject interactionPane;
    public GameObject currencyDisplay;

    private Vector3 unitPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        unitPosition = new Vector3(0, 0, 0);
        ground.SetFloat("_Width", 0);
        interactionPane.SetActive(false);
    }

    public void Update()
    {
        TheGameManager.GameManager gm = TheGameManager.GameManager.instance;
        if (gm.selectedEntity)
        {
            unitPosition.Set(gm.selectedEntity.transform.position.x, 0, gm.selectedEntity.transform.position.z);
            ground.SetVector("_Center", unitPosition);
            ground.SetFloat("_Rotation", gm.selectedEntity.transform.rotation.eulerAngles.y);
        }
    }

    public void SelectEntity(Entity e)
    {
        if(!e)
        {
            ground.SetInt("_BandWidth", 0);
            return;
        }

        ground.SetColor("_BandColor", (e.alignment == TheGameManager.GameManager.instance.playerAlignment) ? allyColor : e.alignment == Globals.Alignment.Neutral ? neutralColor : enemyColor);
        ground.SetInt("_Shape", (int)e.ringShape);
        ground.SetFloat("_Width", e.ringRadius);
        ground.SetFloat("_BandWidth", e.bandWidth);
    }

    public void ShowInteractionPane(Entity selected, Entity target)
    {
        List<Interaction> interactions = InteractionManager.instance.GetInteractions(selected, target);

        interactionPane.transform.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);

        // find the buttons in the pane
        Text[] buttonText = interactionPane.GetComponentsInChildren<Text>(true);
        Button[] buttons = interactionPane.GetComponentsInChildren<Button>(true);
        for(int i = 0; i<buttonText.Length; i++)
        {
            if (i >= interactions.Count)
            {
                buttons[i].gameObject.SetActive(false);
                continue;
            }
            buttons[i].gameObject.SetActive(true);
            buttonText[i].text = interactions[i].GetName();
        }

        interactionPane.SetActive(true);
    }

    public void HideInteractionPane()
    {
        interactionPane.SetActive(false);
    }

    public void UpdateCurrencyWindow(int newAmount)
    {
        Text[] text = currencyDisplay.GetComponentsInChildren<Text>();
        text[1].text = newAmount.ToString();
    }
}
