using System;
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
    }
}
