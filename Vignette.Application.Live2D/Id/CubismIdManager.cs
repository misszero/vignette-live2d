// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vignette.Application.Live2D.Id
{
    public abstract class CubismIdManager
    {
        protected readonly IntPtr Model;

        public CubismIdManager(IntPtr model)
        {
            Model = model;
        }

        public virtual void PreModelUpdate()
        {
        }

        public abstract void PostModelUpdate();
    }

    public abstract class CubismIdManager<T> : CubismIdManager, IEnumerable<T>, IReadOnlyList<T>
        where T : CubismId
    {
        private readonly List<T> items = new List<T>();

        public int Count => items.Count;

        public CubismIdManager(IntPtr model)
            : base(model)
        {
        }

        protected void Add(T id)
        {
            if (Has(id))
                throw new ArgumentException($"ID {id.Id} already exists within this manager.");

            items.Add(id);
        }

        public bool Remove(T id) => items.Remove(id);

        public bool Remove(int index) => items.Remove(items[index]);

        public bool Remove(string name) => items.Remove(Get(name));

        public T Get(string name) => items.FirstOrDefault(id => id.Name == name);

        public bool Has(T id) => items.Any(i => i == id);

        public bool Has(int index) => items.Any(i => i.Id == index);

        public bool Has(string name) => items.Any((id => id.Name == name));

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        public T this[int index] => items[index];
    }
}
