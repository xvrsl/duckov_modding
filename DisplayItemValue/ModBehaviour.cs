using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace DisplayItemValue
{

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static Color White;
        public static Color Green;
        public static Color Blue;
        public static Color Purple;
        public static Color Orange;
        public static Color LightRed;
        public static Color Red;

        TextMeshProUGUI _text = null;
        TextMeshProUGUI Text
        {
            get
            {
                if (_text == null)
                {
                    _text = Instantiate(GameplayDataSettings.UIStyle.TemplateTextUGUI);
                }
                return _text;
            }
        }

        void Awake()
        {
            Debug.Log("DisplayItemValue Loaded!!!");
        }
        void OnDestroy()
        {
            if (_text != null)
                Destroy(_text);
        }
        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;

            ColorUtility.TryParseHtmlString("#FFFFFFff", out White);
            ColorUtility.TryParseHtmlString("#7cff7cff", out Green);
            ColorUtility.TryParseHtmlString("#7cd5ffff", out Blue);
            ColorUtility.TryParseHtmlString("#d0acffff", out Purple);
            ColorUtility.TryParseHtmlString("#ffdc24ff", out Orange);
            ColorUtility.TryParseHtmlString("#ff5858ff", out LightRed);
            ColorUtility.TryParseHtmlString("#ff0000ff", out Red);
        }
        void OnDisable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;
        }

        private void OnSetupMeta(ItemHoveringUI uI, ItemMetaData data)
        {
            Text.gameObject.SetActive(false);
        }

        private void OnSetupItemHoveringUI(ItemHoveringUI uiInstance, Item item)
        {
            if (item == null)
            {
                Text.gameObject.SetActive(false);
                return;
            }

            Text.gameObject.SetActive(true);

            // 计算 价值/自重 比例
            int sellPrice = item.GetTotalRawValue() / 2;
            float ratio = sellPrice / item.SelfWeight;

            // 五段颜色：白 < 绿 < 蓝 < 紫 < 橙 < 玫红 < 大红
            Color32 color;
            if (ratio < 100f) color = White;
            else if (ratio < 500f) color = Green;
            else if (ratio < 1500f) color = Blue; 
            else if (ratio < 3000f) color = Purple;
            else if (ratio < 7000f) color = Orange;
            else if (ratio < 10000f) color = LightRed;
            else color = Red;
            Text.color = color;

            // 设置父级、缩放
            Text.transform.SetParent(uiInstance.LayoutParent, worldPositionStays: false);
            Text.transform.localScale = Vector3.one;

            // 文本：总价和取整后的单价
            Text.text = $"售价：${sellPrice}\n价重比：${(int)ratio}/kg"; 
            
            // 设置字体大小
            Text.fontSize = 20f;
        }
    }
}