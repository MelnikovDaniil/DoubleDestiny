using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public SpriteRenderer ingedientPrefab;
    public float hideTime = 1;

    public float xForce = 25;
    public float yForce = 50;

    public List<IngredientInfo> ingredients;
    public void SpawnIngredient(EnemyScript enemy)
    {
        var randomIngredient = ingredients[Random.Range(0, ingredients.Count)];
        var ingredient = Instantiate(ingedientPrefab, enemy.transform.position, Quaternion.identity, transform);
        ingredient.sprite = randomIngredient.sprite;
        PotionMapper.SetIngredient(randomIngredient.name);

        StartCoroutine(ExecuteAfterTime(ingredient.gameObject));
        var forceVector = new Vector2(Random.Range(-10.0f, 10.0f) * xForce, Random.Range(5f, 10.0f) * yForce);
        ingredient.GetComponent<Rigidbody2D>().AddRelativeForce(forceVector);
    }

    IEnumerator ExecuteAfterTime(GameObject ingredient)
    {
        yield return new WaitForSeconds(hideTime);
        Destroy(ingredient, 1);
        // Code to execute after the delay
    }
}
