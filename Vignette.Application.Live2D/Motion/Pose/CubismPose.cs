// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Pose
{
    public class CubismPose
    {
        public float FadeInTime { get; private set; }

        private readonly IEnumerable<IEnumerable<PartInfo>> groups;

        public CubismPose(CubismModel model, CubismPoseSetting setting)
        {
            FadeInTime = setting.FadeInTime;

            groups = setting.Groups.Select(settingGroup =>
            {
                return settingGroup.Select(settingGroupItem =>
                {
                    return new PartInfo(model.Parts.Get(settingGroupItem.Id))
                    {
                        Children = settingGroupItem.Link.Select(l => model.Parts.Get(l))
                    };
                });
            });

            Reset();
        }

        public void Reset()
        {
            foreach (var group in groups)
            {
                bool first = true;
                foreach (var item in group)
                {
                    float value = first ? 1.0f : 0.0f;
                    item.Parent.Target = value;
                    item.Parent.Target = value;
                    first = false;
                }
            }
        }

        public void Update(float delta)
        {
            foreach (var group in groups)
                doFade(delta, group);

            copyPartOpacities();
        }

        private void copyPartOpacities()
        {
            foreach (var group in groups)
            {
                foreach (var item in group)
                {
                    if (!item.Children.Any())
                        continue;

                    float opacity = item.Parent.Value;
                    foreach (var child in item.Children)
                        child.Value = opacity;
                }
            }
        }

        private void doFade(float delta, IEnumerable<PartInfo> group)
        {
            if (!group.Any())
                return;

            const float epsilon = 0.001f;
            const float phi = 0.5f;
            const float back_opacity_threshold = 0.15f;

            float newOpacity = 1.0f;

            var visible = group.First();
            foreach (var item in group)
            {
                if (epsilon < item.Parent.Target)
                {
                    visible = item;

                    newOpacity = item.Parent.Value;
                    newOpacity += delta / FadeInTime;
                    newOpacity = MathF.Min(newOpacity, 1.0f);
                    break;
                }
            }

            foreach (var item in group)
            {
                if (item == visible)
                    item.Parent.Value = newOpacity;
                else
                {
                    float opacity = item.Parent.Value;
                    float a1 = newOpacity < phi
                        ? newOpacity * (phi - 1.0f) / phi + 1.0f
                        : (1.0f - newOpacity) * phi / (1.0f - phi);

                    float backOpacity = (1.0f - a1) * (1.0f - newOpacity);
                    if (back_opacity_threshold < backOpacity)
                        a1 = 1.0f - back_opacity_threshold / (1.0f - newOpacity);

                    opacity = MathF.Min(opacity, a1);
                    item.Parent.Value = opacity;
                }
            }
        }
    }
}
