using System;
using RedDwarf.Network;

namespace RedDwarf.Server.Events
{
    class ChatMessageEventArgs : EventArgs
    {
        public bool Handled { get; private set; }
        public RemoteClient Origin { get; private set; }
        public ChatMessage Message { get; private set; }

        public ChatMessageEventArgs(RemoteClient origin, ChatMessage message)
        {
            Message = message;
            Origin = origin;
            Handled = false;
        }
    }
}
