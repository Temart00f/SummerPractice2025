using System;
using System.Collections.Generic;
using Interfaces;

namespace task18
{
    public class RoundRobinScheduler : IScheduler
    {
        private readonly Queue<ICommand> commands = new();

        public bool HasCommand()
        {
            return commands.Count > 0;
        }

        public ICommand Select()
        {
            if (commands.Count == 0)
                throw new Exception("No commands available");

            return commands.Dequeue();
        }

        public void Add(ICommand cmd)
        {
            commands.Enqueue(cmd);
        }
    }
}
