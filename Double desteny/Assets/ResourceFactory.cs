using Assets.Interfaces;
using Assets.Minigames.EventCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceFactory : MonoBehaviour
{
    public CameraShake cameraShake;
    public BossHealthBar bossHealthBar;
    public QuickTimeEventManager quickTimeEventManager;
    public ParticleSystem rain;
    public List<GameObject> bossesPresentationPanels;
    public CameraManager cameraManager;
    public CharactersScript characters;
    public MinigameController minigameController;
    public CoinManager coinManager;
    public IngredientManager ingredientManager;
    public CristalManager cristalManager;

    public GameObject EstablishDependencyForEnemy(GameObject obj)
    {
        if (obj.TryGetComponent(out GameEvent gameEvent))
        {
            gameEvent.MinigameController = minigameController;
            gameEvent.CristalManager = cristalManager;
            if (gameEvent is IUseCameraShake)
            {
                var cameraShakeUser = gameEvent as IUseCameraShake;
                cameraShakeUser.CameraShake = cameraShake;
            }
        }

        EnemyScript enemy = null;
        if (obj.TryGetComponent(out enemy) || obj.transform.GetChild(0).TryGetComponent(out enemy))
        {
            if (enemy is IUseCameraShake)
            {
                var cameraShakeUser = enemy as IUseCameraShake;
                cameraShakeUser.CameraShake = cameraShake;
            }
            if (enemy is IUseHealthBar)
            {
                var cameraShakeUser = enemy as IUseHealthBar;
                cameraShakeUser.HealthBar = bossHealthBar;
            }
            if (enemy is IUseQuickTimeEventManager)
            {
                var cameraShakeUser = enemy as IUseQuickTimeEventManager;
                cameraShakeUser.QuickTimeEventManager = quickTimeEventManager;
            }
            if (enemy is IUseRain)
            {
                var cameraShakeUser = enemy as IUseRain;
                cameraShakeUser.Rain = rain;
            }
            if (enemy is IUseResourceFactory)
            {
                var cameraShakeUser = enemy as IUseResourceFactory;
                cameraShakeUser.ResourceFactory = this;
            }
            if (enemy is IUseCamerManager)
            {
                var cameraManagerUser = enemy as IUseCamerManager;
                cameraManagerUser.CameraManager = cameraManager;
            }

            if (enemy is IUseIngredientManager)
            {
                var ingredientUser = enemy as IUseIngredientManager;
                ingredientUser.IngredientManager = ingredientManager;
            }

            enemy.coinManager = coinManager;
        }

        if(obj.TryGetComponent(out BossPresentation bossPresentation))
        {
            bossPresentation.CameraManager = cameraManager;
            bossPresentation.PresentationPanel = bossesPresentationPanels.FirstOrDefault(x => x.name == bossPresentation.Name);
            bossPresentation.characters = characters;
            if (bossPresentation is IUseResourceFactory)
            {
                var wormPresent = bossPresentation as IUseResourceFactory;
                wormPresent.ResourceFactory = this;
            }
        }
        return obj;
    }
}