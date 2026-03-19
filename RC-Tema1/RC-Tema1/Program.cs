using RC_Tema1;
using System.Collections.Concurrent;

int nrStatii = 10;
Node[] nodes = new Node[nrStatii];

for (int i = 0; i < nrStatii; i++)
{
    nodes[i] = new Node(i + 1);
}


for (int i = 0; i < nrStatii; i++)
{
    
    if (i == nrStatii - 1)
    {
        nodes[i].nextNode = nodes[0];
    }
    else
    {
        nodes[i].nextNode = nodes[i + 1];
    }



}



Task[] tasks = new Task[nrStatii];

for(int i = 0;i < nrStatii;i++)
{
    Node nodCurent = nodes[i];
    tasks[i] = Task.Run(() => nodCurent.RunExecution());

}

nodes[2].message = "Salut!";
nodes[2].destination = 7;


nodes[3].message = "Buna";
nodes[3].destination = 7;

TokenRingFrame token = new TokenRingFrame { IsTokenFree = true };
nodes[0].Add(token);
Task.WaitAll(tasks);



