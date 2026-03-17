using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC_Tema1
{
    internal class Node(int id)
    {
        public int Id { get; } = id;
        
        public Node nextNode {  get; set; }

        ConcurrentQueue<TokenRingFrame> buffer = new ConcurrentQueue<TokenRingFrame>();
        public void Add(TokenRingFrame token)
        {
            buffer.Enqueue(token);
        }

        public async void Run()
        {
            while (true)
            {

                bool hasRecived = buffer.TryDequeue(out TokenRingFrame recivedToken);
                if (hasRecived)
                {
                    Process(recivedToken);
                }


            }



        }

        public void Process(TokenRingFrame token)
        {

        }


    }



}
