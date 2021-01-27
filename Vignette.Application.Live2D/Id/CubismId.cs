// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Collections.Generic;

namespace Vignette.Application.Live2D.Id
{
    public class CubismId : ICubismId, IEqualityComparer<CubismId>
    {
        public string Name { get; }

        public int Id { get; }

        public CubismId(string name)
        {
            Name = name;
            Id = -1;
        }

        public CubismId(int index, string name)
        {
            Name = name;
            Id = index;
        }

        public bool Equals(CubismId x, CubismId y) => x.Name == y.Name;

        public int GetHashCode(CubismId obj) => obj.Name.GetHashCode();

        public override string ToString() => $"[{Id}] {Name}";
    }
}
