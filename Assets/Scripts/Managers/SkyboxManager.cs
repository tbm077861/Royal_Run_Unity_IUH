using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem fogParticleSystem; // Kéo Fog Volume vào đây
    [System.Serializable]

    public struct SkyboxProfile
    {
        public string name;
        public Material skyboxMaterial;
        public Color fogColor; // Màu sương mù đi kèm
        public float fogDensity; // Độ dày sương mù (VD: 0.01 -> 0.03)
    }

    [Header("Danh sách các bầu trời")]
    [SerializeField] private SkyboxProfile[] skyboxProfiles;

    void Start()
    {
        ChangeSkyboxRandomly();
    }

    void ChangeSkyboxRandomly()
    {
        if (skyboxProfiles.Length == 0) return;

        // 1. Chọn ngẫu nhiên 1 profile
        int randomIndex = Random.Range(0, skyboxProfiles.Length);
        SkyboxProfile selectedProfile = skyboxProfiles[randomIndex];

        // 2. Thay đổi Skybox
        RenderSettings.skybox = selectedProfile.skyboxMaterial;

        // 3. Thay đổi Fog (Sương mù) để hợp tông màu
        RenderSettings.fog = true;
        RenderSettings.fogColor = selectedProfile.fogColor;
        RenderSettings.fogDensity = selectedProfile.fogDensity;

        // 4. Thay đổi màu của Particle Sương mù (nếu có)
        if (fogParticleSystem != null)
        {
            var main = fogParticleSystem.main;
            // Chỉnh màu particle nhạt hơn màu fog một chút cho tự nhiên
            main.startColor = new ParticleSystem.MinMaxGradient(selectedProfile.fogColor);
        }

        // Cập nhật lại ánh sáng môi trường (Quan trọng để ánh sáng không bị sai màu)
        DynamicGI.UpdateEnvironment();

        Debug.Log($"Đã đổi Skybox sang: {selectedProfile.name}");
    }
}