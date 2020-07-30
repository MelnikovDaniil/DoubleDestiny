using Assets;
using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoesingWay : MonoBehaviour
{
    public Shop shop;
    public GameObject EvilCharacter;
    public GameObject ValorCharacter;
    public Image evilIcon;
    public Image valorIcon;
    public Text evilSkillDescription;
    public Text valorSkillDescription;
    public float guidDelay;

    public void CreateWays(Card card)
    {
        var valorChar = card.character;
        ShowCard(valorChar, ValorCharacter, true);

        var evilChar = card.character;
        ShowCard(evilChar, EvilCharacter, false);
        StartCoroutine(ChoesingWayGuid());
    }

    private IEnumerator ChoesingWayGuid()
    {
        yield return new WaitForSeconds(guidDelay);
        if (GuidMapper.GetStageStatus(GuidStages.ChoosingWay) == GuidStatus.NotActive)
        {
            GuidManager.Instance.ActivateStage(GuidStages.ChoosingWay);
        }
    }

    public void ShowCard(Char character, GameObject characterPlace, bool isValor)
    {
        var valorParent = characterPlace.transform.parent;
        Destroy(characterPlace);
        characterPlace = Instantiate(character.gameObject, valorParent);
        characterPlace.transform.localPosition = Vector2.zero;
        characterPlace.transform.localScale = new Vector3(90, 90, 90);
        var newCharacter = characterPlace.GetComponent<Char>();
        newCharacter.nonPlayed = true;

        if (isValor)
        {
            newCharacter.TransformToValor();
            valorIcon.sprite = newCharacter.valorSkillIcon;
            valorSkillDescription.text = newCharacter.valorSkillDescription;
            ValorCharacter = characterPlace;

        }
        else
        {
            newCharacter.TransformToEvil();
            evilIcon.sprite = newCharacter.evilSkillIcon;
            evilSkillDescription.text = newCharacter.evilSkillDescription;
            EvilCharacter = characterPlace;
        }

        
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    public void ValorWin()
    {
        shop.currentCard.ChoesWay(true);
    }

    public void EvilWin()
    {
        shop.currentCard.ChoesWay(false);
    }
}
