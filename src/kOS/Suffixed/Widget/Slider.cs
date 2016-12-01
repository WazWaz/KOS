﻿using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;

namespace kOS.Suffixed
{
    [kOS.Safe.Utilities.KOSNomenclature("Slider")]
    public class Slider : Widget
    {
        public bool horizontal { get; set; }
        private float value { get; set; }
        private float value_visible { get; set; }
        public float min { get; set; }
        public float max { get; set; }
        protected GUIStyle thumbStyle;

        public Slider(Box parent, bool h_not_v, float v, float from, float to) : base(parent)
        {
            RegisterInitializer(InitializeSuffixes);
            horizontal = h_not_v;
            value = v;
            min = from;
            max = to;
            if (horizontal) { setstyle.margin.top = 8; setstyle.margin.bottom = 8; } // align better with labels.
            thumbStyle = new GUIStyle(horizontal ? HighLogic.Skin.horizontalSliderThumb : HighLogic.Skin.verticalSliderThumb);
        }

        protected override GUIStyle BaseStyle() { return horizontal ? HighLogic.Skin.horizontalSlider : HighLogic.Skin.verticalSlider; }

        private void InitializeSuffixes()
        {
            AddSuffix("VALUE", new SetSuffix<ScalarValue>(() => value, v => { if (value != v) { value = v; Communicate(() => value_visible = v); } }));
            AddSuffix("MIN", new SetSuffix<ScalarValue>(() => min, v => min = v));
            AddSuffix("MAX", new SetSuffix<ScalarValue>(() => max, v => max = v));
        }

        public override void DoGUI()
        {
            float newvalue;
            if (horizontal)
                newvalue = GUILayout.HorizontalSlider(value_visible, min, max, style, thumbStyle);
            else
                newvalue = GUILayout.VerticalSlider(value_visible, min, max, style, thumbStyle);
            if (newvalue != value_visible) {
                value_visible = newvalue;
                Communicate(() => value = newvalue);
            }
        }

        public override string ToString()
        {
            return string.Format("SLIDER({0:0.00})",value);
        }
    }
}