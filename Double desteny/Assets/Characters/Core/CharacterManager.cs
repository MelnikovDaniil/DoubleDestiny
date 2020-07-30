using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<Char> characters;
    public CharactersScript charactersScript;
    public float movingTime = 1;

    private float currentMovingTime;
    private float coof;
    private Vector2 rangerStartPosition = new Vector3(-15, 0, 0);
    private Vector2 rangerFinalPosition = new Vector2(-1.2f, 0.3f);

    private Vector2 wariorStartPosition = new Vector3(15, 0, 0);

    public void SetCharacter()
    {
        var ranger = characters.First(x => x.Name == CharactersMapper.GetCurrentRanger());
        var warior = characters.First(x => x.Name == CharactersMapper.GetCurrentWarrior());
        charactersScript.ranger = ranger.gameObject;
        charactersScript.warior = warior.gameObject;
    }

    public void MoveCharacters()
    {
        charactersScript.ranger.transform.localPosition = rangerStartPosition;
        charactersScript.warior.transform.localPosition = wariorStartPosition;
        currentMovingTime = movingTime;
    }

    private void Update()
    {
        if (currentMovingTime > 0)
        {
            currentMovingTime -= Time.deltaTime;
            coof = currentMovingTime / movingTime;
            charactersScript.ranger.transform.localPosition = Vector2.Lerp(rangerFinalPosition, rangerStartPosition, coof);
            charactersScript.warior.transform.localPosition = Vector2.Lerp(Vector2.zero, wariorStartPosition, coof);
        }
    }
}
