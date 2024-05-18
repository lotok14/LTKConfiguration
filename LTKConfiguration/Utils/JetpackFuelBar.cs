using LTKConfiguration.Patches;
using UnityEngine;
using UnityEngine.UI;

namespace LTKConfiguration.Utils
{
    public class JetpackFuelBar
    {
        private float fuelLeft;
        private float maxFuelAmount;
        private GameObject fuelBarCanvas;
        private Slider fuelBarSlider;
        private Character parentCharacter;

        public void resetFuelAmount(float maxFuelAmount)
        {
            this.maxFuelAmount = maxFuelAmount;
            this.fuelLeft = maxFuelAmount;

            if (this.fuelBarCanvas == null)
            {
                InstantiateFuelBarPrefab();
            }
            else
            {
                this.fuelBarSlider.value = 1;
            }
        }

        public float useFuel(float useAmount)
        {
            this.fuelLeft -= useAmount;
            this.fuelBarSlider.value = fuelLeftPercentage() / 100;
            if(this.fuelLeft <= 0)
            {
                UnityEngine.Object.Destroy(this.fuelBarCanvas);
            }
            return this.fuelLeft;
        }   

        public float fuelLeftPercentage()
        {
            return this.fuelLeft / this.maxFuelAmount * 100;
        }

        public void setParentCharacter(Character parentCharacter)
        {
            if (this.parentCharacter == null)
            {
                this.parentCharacter = parentCharacter;
            }
        }

        private void InstantiateFuelBarPrefab()
        {
            // instantiate fuelBarCanvas
            this.fuelBarCanvas = GameObject.Instantiate(JetpackPatch.fuelBarPrefab, this.parentCharacter.transform, false);
            this.fuelBarCanvas.transform.localPosition = new Vector3(0, -1, 0);
            // Add reference to slider
            this.fuelBarSlider = this.fuelBarCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        }
    }
}

