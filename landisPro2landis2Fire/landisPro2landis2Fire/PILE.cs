public class PILE

//Pile ADT.  Accepts POINT as input and output.



{




public PILE()

//Constructor.



{

numItems = 0;

}

public int isEmpty()

//This returns true if the pile is empty, false otherwise.



{

return numItems == 0;

}

public int push(LDPOINT p)

//Pushes p onto the pile.  Returns true if unsuccesful, false otherwise.



{

if (numItems < (DefineConstants.MAXPILE-1))

{

	items[numItems++] = p;

	return false;

}

else

{

	errorSys("MAXIMUM PILE SPACE EXCEEDED!.",DefineConstants.CONTINUE);

	return true;

}

}

					//false otherwise.

public LDPOINT pull()

//Pulls p off of the pile.



{

LDPOINT p = new LDPOINT();

int pos = (int)(frand() * (float)numItems);

if (pos < 0 || pos >= numItems)
{

	pos = 0;
}

p.CopyFrom(items[pos]);

for (int i = pos;i < numItems - 1;i++)
{

   items[i] = items[i + 1];
}

numItems--;

return p;

}

public void reset()

//This will reset the pile to an empty state.



{

numItems = 0;

}






private LDPOINT[] items = Arrays.InitializeWithDefaultInstances<LDPOINT>(DefineConstants.MAXPILE); //Array of pile items;

private int numItems; //Number of items on pile.

}

