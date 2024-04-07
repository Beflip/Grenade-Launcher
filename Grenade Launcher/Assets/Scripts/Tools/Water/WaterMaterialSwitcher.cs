namespace Assets.Scripts.Water
{
    using UnityEngine;
    public class WaterMaterialSwitcher : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material waterMaterial;
        [SerializeField] private Material diffuseMaterial;

        private MaterialPropertyBlock defaulPropertyBlock;

        public void Awake()
        {
            defaulPropertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(defaulPropertyBlock);
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Water")
            {
                var waterPropertyBlock = collider.GetComponent<WaterArea>().WaterPropertyBlock;

                _renderer.sharedMaterial = waterMaterial;
                _renderer.SetPropertyBlock(waterPropertyBlock);
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            if (collider.tag == "Water")
            {
                _renderer.sharedMaterial = diffuseMaterial;
                _renderer.SetPropertyBlock(defaulPropertyBlock);
            }
        }
    }
}
