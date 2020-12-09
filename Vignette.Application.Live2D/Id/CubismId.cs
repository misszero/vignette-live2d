// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;

namespace Vignette.Application.Live2D.Id
{
    public class CubismId : ICubismId, IEqualityComparer<CubismId>
    {
        public string Name { get; }

        public int Index { get; }

        public CubismId(string name)
        {
            Name = name;
            Index = -1;
        }

        public CubismId(int index, string name)
        {
            Name = name;
            Index = index;
        }

        public bool Equals(CubismId x, CubismId y) => x.Name == y.Name;

        public int GetHashCode(CubismId obj) => obj.Name.GetHashCode();

        public override string ToString() => $"[{Index}] {Name}";
    }
}
