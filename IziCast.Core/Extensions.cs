using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IziCast.Core
{
    public static class Extensions
    {
        public static void SafeInvoke<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
        {
            if (eventHandler == null)
                return;

            eventHandler.Invoke(sender, e);
        }

        public static void SafeInvoke(this EventHandler eventHandler, object sender, EventArgs e)
        {
            if (eventHandler == null)
                return;

            eventHandler.Invoke(sender, e);
        }

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> collection)
        {
            if (!(collection is IList<T> list))
                list = collection.ToList();

            return new ReadOnlyCollection<T>(list);
        }
    }
}
