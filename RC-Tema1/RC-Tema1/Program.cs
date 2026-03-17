using RC_Tema1;
using System.Collections.Concurrent;

int nrStatii = 10;
Node[] noduri = new Node[nrStatii];

for (int i = 0; i < nrStatii; i++)
{
    noduri[i] = new Node(i + 1);
}


for (int i = 0; i < nrStatii; i++)
{
    
    if (i == nrStatii - 1)
    {
        noduri[i].nextNode = noduri[0];
    }
    else
    {
        noduri[i].nextNode = noduri[i + 1];
    }
}

