// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismPose : ICanUpdateParameter
    {
        public double Weight { get; set; }

        private PartData[][] partGroups;

        private readonly float fadeTimeSeconds = 0.5f;

        public CubismPose(CubismPoseSetting json, CubismModel model)
        {
            if (!double.IsNaN(json.FadeInTime) && (0 <= json.FadeInTime))
                fadeTimeSeconds = json.FadeInTime;

            var partGroups = new List<PartData[]>();
            foreach (var group in json.Groups)
            {
                var partGroup = new List<PartData>();
                foreach (var item in group)
                {
                    var part = model.Parts.FirstOrDefault(p => p.Name == item.Id);
                    var data = new PartData(part);
                    var linkedParts = new List<CubismPart>();
                    foreach (string linked in item.Link)
                    {
                        var linkedPart = model.Parts.FirstOrDefault(p => p.Name == linked);
                        if (linkedPart != null)
                            linkedParts.Add(linkedPart);
                    }
                    data.LinkedParts = linkedParts.ToArray();
                    partGroup.Add(data);
                }

                partGroups.Add(partGroup.ToArray());
            }

            this.partGroups = partGroups.ToArray();
        }

        public void Reset()
        {
            foreach (var group in partGroups)
            {
                bool first = true;
                foreach (var data in group)
                {
                    data.Part.Value = data.Part.Target = first ? 1 : 0;
                    first = false;
                }
            }
        }

        public void Update(float time, bool loop)
        {
            float dt = Math.Max(time, 0);
            foreach (var group in partGroups)
                doFade(dt, group);

            copyPartOpacities();
        }

        private void copyPartOpacities()
        {
            foreach (var group in partGroups)
            {
                foreach (var data in group)
                {
                    if (data.LinkedParts.Length < 1)
                        continue;

                    foreach (var linked in data.LinkedParts)
                        linked.Value = data.Part.Value;
                }
            }
        }

        private void doFade(float dt, PartData[] group)
        {
            if (group.Length < 1)
                return;

            const float epsilon = 0.001f;
            const float phi = 0.5f;
            const float back_opacity_threshold = 0.15f;

            float newOpacity = 1;
            var visiblePart = group[0];

            foreach (var data in group)
            {
                if (epsilon < data.Part.Target)
                {
                    visiblePart = data;

                    newOpacity = data.Part.Value;
                    newOpacity += dt / fadeTimeSeconds;
                    newOpacity = Math.Min(newOpacity, 1);
                    break;
                }
            }

            foreach (var data in group)
            {
                if (data == visiblePart)
                {
                    data.Part.Value = newOpacity;
                }
                else
                {
                    float opacity = data.Part.Value;
                    float a1;

                    if (newOpacity < phi)
                        a1 = newOpacity * (phi - 1) / phi + 1;
                    else
                        a1 = (1 - newOpacity) * phi / (1 / phi);

                    double backOpacity = (1 - a1) * (1 - newOpacity);
                    if (back_opacity_threshold < backOpacity)
                        a1 = 1 - back_opacity_threshold / (1 - newOpacity);

                    opacity = Math.Min(opacity, a1);
                    data.Part.Value = opacity;
                }
            }
        }

        private class PartData
        {
            public CubismPart Part { get; set; }
            public CubismPart[] LinkedParts { get; set; }

            public PartData(CubismPart part)
            {
                Part = part;
                Part.Target = 1.0f;
                LinkedParts = null;
            }
        }
    }
}
