// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vignette.Application.Live2D.Id
{
    public abstract class CubismIdManager<T> : IEnumerable<T>, IReadOnlyList<T>
        where T : CubismId
    {
        private readonly List<T> items = new List<T>();

        protected readonly IntPtr Model;

        public int Count => items.Count;

        public CubismIdManager(IntPtr model)
        {
            Model = model;
        }

        protected void Add(T id)
        {
            if (Has(id))
                throw new ArgumentException($"ID {id.Index} already exists within this manager.");

            items.Add(id);
        }

        public bool Remove(T id) => items.Remove(id);

        public bool Remove(int index) => items.Remove(items[index]);

        public bool Remove(string name) => items.Remove(Get(name));

        public T Get(string name) => items.FirstOrDefault(id => id.Name == name);

        public bool Has(T id) => items.Any(i => i == id);

        public bool Has(int index) => items.Any(i => i.Index == index);

        public bool Has(string name) => items.Any((id => id.Name == name));

        public virtual void PreModelUpdate()
        {
        }

        public abstract void PostModelUpdate();

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        public T this[int index] => items[index];
    }
}
