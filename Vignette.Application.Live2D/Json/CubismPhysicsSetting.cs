// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;

namespace Vignette.Application.Live2D.Json
{
    public class CubismPhysicsSetting : ICubismJsonSetting
    {
        public int Version { get; set; }

        public Metadata Meta { get; set; }

        public List<PhysicsSetting> PhysicsSettings { get; set; }

        public class Metadata
        {
            public int PhysicsSettingCount { get; set; }

            public int TotalInputCount { get; set; }

            public int TotalOutputCount { get; set; }

            public int VertexCount { get; set; }

            public EffectiveForce EffectiveForces { get; set; }

            public List<PhysicsDictionaryItem> PhysicsDictionary { get; set; }

            public class EffectiveForce
            {
                public Vector2D Gravity { get; set; }

                public Vector2D Wind { get; set; }
            }

            public class PhysicsDictionaryItem
            {
                public string Id { get; set; }

                public string Name { get; set; }
            }
        }

        public class PhysicsSetting
        {
            public string Id { get; set; }

            public List<InputSetting> Input { get; set; }

            public List<OutputSetting> Output { get; set; }

            public List<VertexSetting> Vertices { get; set; }

            public NormalizationSetting Normalization { get; set; }

            public class InputSetting
            {
                public SourceDestination Source { get; set; }

                public double Weight { get; set; }

                public string Type { get; set; }

                public bool Reflect { get; set; }
            }

            public class OutputSetting
            {
                public SourceDestination Destination { get; set; }

                public int VertexIndex { get; set; }

                public double Scale { get; set; }

                public double Weight { get; set; }

                public string Type { get; set; }

                public bool Reflect { get; set; }
            }

            public class VertexSetting
            {
                public Vector2D Position { get; set; }

                public double Mobility { get; set; }

                public double Delay { get; set; }

                public double Acceleration { get; set; }

                public double Radius { get; set; }
            }

            public class NormalizationSetting
            {
                public MinMaxSetting Position { get; set; }

                public MinMaxSetting Angle { get; set; }
            }

            public class SourceDestination
            {
                public string Target { get; set; }

                public string Id { get; set; }
            }

            public class MinMaxSetting
            {
                public double Minimum { get; set; }

                public double Default { get; set; }

                public double Maximum { get; set; }
            }
        }

        public class Vector2D
        {
            public double X { get; set; }

            public double Y { get; set; }
        }
    }
}
