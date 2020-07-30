using Assets.Mappers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Runes
{
    public class Boiler : MonoBehaviour
    {
        public float timeLeft;

        public RuneInfo rune;

        [SerializeField]
        private RGBPenka particles;
        [SerializeField]
        private Button skipButton;

        public RuneView runeView;

        public void CookRune(RuneView runeView)
        {
            this.runeView = runeView;
            rune = runeView.rune;
            particles.currentColor = rune.color;
            runeView.redCristallImage.transform.parent.gameObject.SetActive(false);
            runeView.greenCristallImage.transform.parent.gameObject.SetActive(false);
            runeView.blueCristallImage.transform.parent.gameObject.SetActive(false);
            runeView.time.enabled = true;
            runeView.progressBar.enabled = true;
            particles.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
        }

        public void StopCooking()
        {
            runeView.redCristallImage.transform.parent.gameObject.SetActive(true);
            runeView.greenCristallImage.transform.parent.gameObject.SetActive(true);
            runeView.blueCristallImage.transform.parent.gameObject.SetActive(true);
            runeView.time.enabled = false;
            runeView.progressBar.enabled = false;
            runeView = null;
            particles.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            RuneMapper.UpgradeLevel(rune.name);
            RuneMapper.DisableTime(rune.name);
        }
    }
}
