using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace EternalKingdoms
{
    public class EternalKingdomsBootstrap : MonoBehaviour
    {
        private static bool created;
        private Canvas canvas;
        private Font font;
        private readonly Color bg = new Color(0.035f, 0.045f, 0.075f);
        private readonly Color panel = new Color(0.075f, 0.09f, 0.14f);
        private readonly Color gold = new Color(0.88f, 0.68f, 0.30f);
        private readonly Color muted = new Color(0.68f, 0.70f, 0.76f);
        private readonly List<GameObject> screenObjects = new List<GameObject>();
        private string currentScreen = "Inicio";
        private float musicVolume = 0.7f;
        private float sfxVolume = 0.8f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            if (created) return;
            if (FindObjectOfType<EternalKingdomsBootstrap>() != null) { created = true; return; }
            var go = new GameObject("EternalKingdoms_UI");
            go.AddComponent<EternalKingdomsBootstrap>();
        }

        private void Awake()
        {
            if (created && FindObjectOfType<EternalKingdomsBootstrap>() != this) { Destroy(gameObject); return; }
            created = true;
            DontDestroyOnLoad(gameObject);
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            BuildCanvas();
            Show("Inicio");
        }

        private void BuildCanvas()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                DontDestroyOnLoad(es);
            }

            var cgo = new GameObject("Eternal Kingdoms Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            cgo.transform.SetParent(transform, false);
            canvas = cgo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = cgo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        private void ClearScreen()
        {
            foreach (var obj in screenObjects) if (obj != null) Destroy(obj);
            screenObjects.Clear();
        }

        private GameObject Root(string name)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(canvas.transform, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            go.GetComponent<Image>().color = bg;
            screenObjects.Add(go);
            return go;
        }

        private Text MakeText(Transform parent, string value, int size, Color color, TextAnchor align, Vector2 pos, Vector2 dims, bool bold = false)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos; rt.sizeDelta = dims;
            var t = go.GetComponent<Text>();
            t.text = value; t.font = font; t.fontSize = size; t.color = color; t.alignment = align;
            t.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }

        private void MakePanel(Transform parent, Vector2 pos, Vector2 size, Color color)
        {
            var go = new GameObject("Panel", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos; rt.sizeDelta = size;
            go.GetComponent<Image>().color = color;
        }

        private void MakeButton(Transform parent, string label, Vector2 pos, Vector2 size, UnityEngine.Events.UnityAction action, bool primary = false)
        {
            var go = new GameObject("Button_" + label, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos; rt.sizeDelta = size;
            var image = go.GetComponent<Image>();
            image.color = primary ? new Color(0.48f, 0.31f, 0.12f) : new Color(0.12f, 0.15f, 0.21f);
            var button = go.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(action);
            MakeText(go.transform, label, 22, primary ? Color.white : gold, TextAnchor.MiddleCenter, Vector2.zero, size - new Vector2(12, 8), true);
        }

        private void Header(Transform root, string title, string subtitle = "")
        {
            MakeText(root.transform, "ETERNAL KINGDOMS", 19, gold, TextAnchor.MiddleLeft, new Vector2(-730, 485), new Vector2(400, 45), true);
            MakeText(root.transform, title.ToUpper(), 42, Color.white, TextAnchor.MiddleCenter, new Vector2(0, 420), new Vector2(900, 65), true);
            if (!string.IsNullOrEmpty(subtitle))
                MakeText(root.transform, subtitle, 18, muted, TextAnchor.MiddleCenter, new Vector2(0, 375), new Vector2(1000, 40));
            MakeButton(root.transform, "VOLVER", new Vector2(-790, -475), new Vector2(170, 58), () => Show("Inicio"));
        }

        public void Show(string screen)
        {
            currentScreen = screen;
            ClearScreen();
            var root = Root(screen + "_Root");
            if (screen == "Inicio") BuildHome(root);
            else if (screen == "Jugar") BuildLobby(root);
            else if (screen == "Héroes") BuildHeroes(root);
            else if (screen == "Tienda") BuildStore(root);
            else if (screen == "Colección") BuildCollection(root);
            else if (screen == "Perfil") BuildProfile(root);
            else if (screen == "Eventos") BuildEvents(root);
            else if (screen == "Configuración") BuildSettings(root);
        }

        private void BuildHome(GameObject root)
        {
            MakeText(root.transform, "ETERNAL", 72, gold, TextAnchor.MiddleCenter, new Vector2(0, 285), new Vector2(900, 100), true);
            MakeText(root.transform, "KINGDOMS", 54, Color.white, TextAnchor.MiddleCenter, new Vector2(0, 220), new Vector2(900, 75), true);
            MakeText(root.transform, "DOS REINOS. UNA GUERRA. DIEZ HÉROES.", 20, muted, TextAnchor.MiddleCenter, new Vector2(0, 160), new Vector2(900, 40));
            MakePanel(root.transform, new Vector2(0, -20), new Vector2(520, 470), new Color(0.055f, 0.07f, 0.11f, 0.94f));
            MakeButton(root.transform, "JUGAR", new Vector2(0, 145), new Vector2(390, 68), () => Show("Jugar"), true);
            MakeButton(root.transform, "HÉROES", new Vector2(0, 60), new Vector2(390, 58), () => Show("Héroes"));
            MakeButton(root.transform, "COLECCIÓN", new Vector2(0, -12), new Vector2(390, 58), () => Show("Colección"));
            MakeButton(root.transform, "TIENDA", new Vector2(0, -84), new Vector2(390, 58), () => Show("Tienda"));
            MakeButton(root.transform, "PERFIL", new Vector2(-620, -440), new Vector2(190, 64), () => Show("Perfil"));
            MakeButton(root.transform, "EVENTOS", new Vector2(0, -440), new Vector2(190, 64), () => Show("Eventos"));
            MakeButton(root.transform, "AJUSTES", new Vector2(620, -440), new Vector2(190, 64), () => Show("Configuración"));
            MakeText(root.transform, "TEMPORADA I  •  EL DESPERTAR DE LOS REINOS", 16, muted, TextAnchor.MiddleCenter, new Vector2(0, -350), new Vector2(900, 35));
            MakeText(root.transform, "PROTOTIPO DE INTERFAZ — CONTENIDO DE DEMOSTRACIÓN", 13, muted, TextAnchor.MiddleCenter, new Vector2(0, -510), new Vector2(900, 25));
        }

        private void BuildLobby(GameObject root)
        {
            Header(root, "Sala de batalla", "Partida personalizada • Vista previa 5 vs 5");
            MakePanel(root.transform, new Vector2(0, -25), new Vector2(1450, 620), panel);
            for (int i = 0; i < 5; i++)
            {
                float y = 190 - i * 92;
                MakePanel(root.transform, new Vector2(-350, y), new Vector2(560, 72), new Color(0.10f, 0.16f, 0.19f));
                MakeText(root.transform, "ALIADO " + (i + 1) + "     Héroe por elegir", 19, Color.white, TextAnchor.MiddleLeft, new Vector2(-350, y), new Vector2(520, 55));
                MakePanel(root.transform, new Vector2(350, y), new Vector2(560, 72), new Color(0.19f, 0.11f, 0.13f));
                MakeText(root.transform, "ENEMIGO " + (i + 1) + "     Sin seleccionar", 19, Color.white, TextAnchor.MiddleLeft, new Vector2(350, y), new Vector2(520, 55));
            }
            MakeText(root.transform, "TU EQUIPO", 22, new Color(0.45f, 0.85f, 0.75f), TextAnchor.MiddleCenter, new Vector2(-350, 260), new Vector2(500, 40), true);
            MakeText(root.transform, "EQUIPO RIVAL", 22, new Color(0.95f, 0.45f, 0.45f), TextAnchor.MiddleCenter, new Vector2(350, 260), new Vector2(500, 40), true);
            MakeButton(root.transform, "ELEGIR HÉROE", new Vector2(0, -390), new Vector2(260, 60), () => Show("Héroes"), true);
        }

        private void BuildHeroes(GameObject root)
        {
            Header(root, "Héroes", "Diez campeones aguardan su llamado. Selección de demostración.");
            string[] names = { "Aldric", "Brakka", "Seraphine", "Elyra", "Kael", "Nyx", "Thorne", "Mira", "Darius", "Sylva" };
            string[] roles = { "Guerrero", "Tanque", "Mago", "Mago", "Asesino", "Asesino", "Tanque", "Tirador", "Guerrero", "Tirador" };
            for (int i = 0; i < names.Length; i++)
            {
                int col = i % 5, row = i / 5;
                Vector2 p = new Vector2(-650 + col * 325, 100 - row * 260);
                MakePanel(root.transform, p, new Vector2(280, 205), new Color(0.10f, 0.12f, 0.18f));
                MakeText(root.transform, "✦", 42, gold, TextAnchor.MiddleCenter, p + new Vector2(0, 38), new Vector2(80, 60), true);
                MakeText(root.transform, names[i], 23, Color.white, TextAnchor.MiddleCenter, p + new Vector2(0, -22), new Vector2(250, 38), true);
                MakeText(root.transform, roles[i] + "  •  2 skins", 15, muted, TextAnchor.MiddleCenter, p + new Vector2(0, -62), new Vector2(250, 30));
            }
        }

        private void BuildStore(GameObject root)
        {
            Header(root, "Tienda", "Objetos cosméticos y campeones • Catálogo de ejemplo");
            string[] items = { "Skin: Guardián Dorado", "Skin: Hechicera Lunar", "Avatar: León Real", "Marco: Corona Antigua", "Héroe: Kael", "Efecto: Llama Arcana" };
            for (int i = 0; i < items.Length; i++)
            {
                Vector2 p = new Vector2(-420 + (i % 3) * 420, 125 - (i / 3) * 260);
                MakePanel(root.transform, p, new Vector2(350, 205), panel);
                MakeText(root.transform, "✧", 48, gold, TextAnchor.MiddleCenter, p + new Vector2(0, 38), new Vector2(100, 65));
                MakeText(root.transform, items[i], 19, Color.white, TextAnchor.MiddleCenter, p + new Vector2(0, -35), new Vector2(320, 40), true);
                MakeText(root.transform, "DISPONIBLE PRÓXIMAMENTE", 13, muted, TextAnchor.MiddleCenter, p + new Vector2(0, -72), new Vector2(300, 28));
            }
        }

        private void BuildCollection(GameObject root)
        {
            Header(root, "Colección", "Tus héroes, apariencias y elementos cosméticos.");
            string[] categories = { "HÉROES", "SKINS", "AVATARES", "MARCOS", "EFECTOS" };
            for (int i = 0; i < categories.Length; i++)
            {
                MakeButton(root.transform, categories[i], new Vector2(-500 + i * 250, 245), new Vector2(220, 58), () => { });
            }
            MakePanel(root.transform, new Vector2(0, -40), new Vector2(1250, 430), panel);
            MakeText(root.transform, "TU COLECCIÓN", 30, gold, TextAnchor.MiddleCenter, new Vector2(0, 85), new Vector2(600, 55), true);
            MakeText(root.transform, "Todavía no has desbloqueado objetos.\nLos elementos obtenidos aparecerán aquí.", 21, muted, TextAnchor.MiddleCenter, new Vector2(0, -20), new Vector2(800, 100));
        }

        private void BuildProfile(GameObject root)
        {
            Header(root, "Perfil de jugador", "Identidad y estadísticas de demostración.");
            MakePanel(root.transform, new Vector2(0, 15), new Vector2(1050, 520), panel);
            MakePanel(root.transform, new Vector2(-360, 90), new Vector2(190, 190), new Color(0.22f, 0.17f, 0.10f));
            MakeText(root.transform, "EK", 60, gold, TextAnchor.MiddleCenter, new Vector2(-360, 90), new Vector2(160, 120), true);
            MakeText(root.transform, "Aventurero#0001", 30, Color.white, TextAnchor.MiddleLeft, new Vector2(30, 170), new Vector2(550, 50), true);
            MakeText(root.transform, "Nivel 1  •  Sin rango  •  ID: EK-000001", 18, muted, TextAnchor.MiddleLeft, new Vector2(30, 120), new Vector2(550, 40));
            MakeText(root.transform, "VICTORIAS      0       DERROTAS      0       WIN RATE      0%", 23, gold, TextAnchor.MiddleCenter, new Vector2(0, -80), new Vector2(850, 60), true);
            MakeText(root.transform, "Historial de partidas: todavía no hay partidas registradas.", 17, muted, TextAnchor.MiddleCenter, new Vector2(0, -175), new Vector2(850, 45));
        }

        private void BuildEvents(GameObject root)
        {
            Header(root, "Eventos", "Aventuras y celebraciones del reino.");
            string[] events = { "El despertar de los reinos", "Desafío de los campeones", "Festival de la luna" };
            for (int i = 0; i < events.Length; i++)
            {
                Vector2 p = new Vector2(-430 + i * 430, 0);
                MakePanel(root.transform, p, new Vector2(360, 340), panel);
                MakeText(root.transform, "✦", 64, gold, TextAnchor.MiddleCenter, p + new Vector2(0, 75), new Vector2(100, 80));
                MakeText(root.transform, events[i], 22, Color.white, TextAnchor.MiddleCenter, p + new Vector2(0, 5), new Vector2(320, 60), true);
                MakeText(root.transform, "Próximamente", 17, muted, TextAnchor.MiddleCenter, p + new Vector2(0, -65), new Vector2(280, 35));
            }
        }

        private void BuildSettings(GameObject root)
        {
            Header(root, "Configuración", "Preferencias locales de demostración.");
            MakePanel(root.transform, new Vector2(0, 0), new Vector2(1000, 560), panel);
            MakeText(root.transform, "Volumen de música", 22, Color.white, TextAnchor.MiddleLeft, new Vector2(-270, 160), new Vector2(400, 45));
            MakeText(root.transform, "Volumen de efectos", 22, Color.white, TextAnchor.MiddleLeft, new Vector2(-270, 50), new Vector2(400, 45));
            MakeText(root.transform, "Calidad gráfica", 22, Color.white, TextAnchor.MiddleLeft, new Vector2(-270, -60), new Vector2(400, 45));
            MakeButton(root.transform, "MÚSICA  -", new Vector2(180, 160), new Vector2(150, 55), () => { musicVolume = Mathf.Max(0, musicVolume - .1f); AudioListener.volume = musicVolume; });
            MakeButton(root.transform, "MÚSICA  +", new Vector2(360, 160), new Vector2(150, 55), () => { musicVolume = Mathf.Min(1, musicVolume + .1f); AudioListener.volume = musicVolume; });
            MakeButton(root.transform, "EFECTOS  -", new Vector2(180, 50), new Vector2(150, 55), () => { sfxVolume = Mathf.Max(0, sfxVolume - .1f); });
            MakeButton(root.transform, "EFECTOS  +", new Vector2(360, 50), new Vector2(150, 55), () => { sfxVolume = Mathf.Min(1, sfxVolume + .1f); });
            MakeButton(root.transform, "BAJA", new Vector2(180, -60), new Vector2(150, 55), () => QualitySettings.SetQualityLevel(0));
            MakeButton(root.transform, "ALTA", new Vector2(360, -60), new Vector2(150, 55), () => QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1));
            MakeText(root.transform, "Orientación prevista: horizontal (Landscape)", 17, muted, TextAnchor.MiddleCenter, new Vector2(0, -180), new Vector2(700, 40));
        }
    }
}
