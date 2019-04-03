using System;

// FireRegimeUnit.cpp: implementation of the CFireRegimeUnit class.

//

//////////////////////////////////////////////////////////////////////



// FireRegimeUnit.h: interface for the CFireRegimeUnit class.

//

//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000


#endif















public class CFireRegimeUnit : System.IDisposable

{


	public string name; //Fire Regime Unit name.

	public int index; //0, 1, 2, ...

	public int fireInterval; //This holds fire classes in conjunction with fire
	public int initialLastFire;
	public int[] windCurve = new int[5];
	public int[] windClass = new int[5];
	public int[] fireCurve = new int[5];
	public int[] fireClass = new int[5];
							   //element stores the number of years fuel must
							   //accumulate for the LANDUNIT to accomodate a fire
							   //of a given severity.

							   //curve.



		   //windCurve[5],       //Same as fireCurve and fireClass but used for fire

		   //I should delete wind curve info. J.Yang 2004-2-17

		   //windClass[5];       //fuel after windthrow.

			// I also should delete windClass info. J.Yang 2004-2-17



	public float m_fIgPoisson; //Fire ignition Poisson parameter

	//average # of ignition per ha (10,000 m2) per decade

	public float m_fMFS; // mean fire size in this landtype (ha)

	public float m_fFireSTD; //fire size variance squar root in this landtype (ha)






	//////////////////////////////////////////////////////////////////////

	// Construction/Destruction

	//////////////////////////////////////////////////////////////////////



	public CFireRegimeUnit()

	{

	 name = null;

	 m_fIgPoisson = 0.0F;

	 m_fMFS = 0.0F;

	 m_fFireSTD = 0.0F;

	 fireInterval = 0;



	}

	public virtual void Dispose()

	{

	if (name != 0)
	{

		Arrays.DeleteArray(name); //Nim: changed delete to delete []
	}



	}



	public void read(FILE infile)

	{

		int i;

		string instring = new string(new char[255]);

		if (fscanc(infile,"%s",instring) != 1)
		{

			errorSys("Error reading in name from landtype file.",DefineConstants.STOP);
		}



		if (name != 0)
		{

			name = null;
		}

		name = new string(new char[instring.Length]);

		name = instring;

		if (fscanc(infile, "%d", fireInterval) != 1)
		{

			errorSys("Error reading in fireInterval from landtype file.",DefineConstants.STOP);
		}

		if (fscanc(infile, "%f", m_fIgPoisson) != 1)
		{

			errorSys("Error reading in fire ignition poisson parameter from landtype file.",DefineConstants.STOP);
		}

		if (fscanc(infile, "%f", m_fMFS) != 1)
		{

			errorSys("Error reading in MFS from landtype file.",DefineConstants.STOP);
		}

		if (fscanc(infile, "%f", m_fFireSTD) != 1)
		{

			errorSys("Error reading in fire size Variance from landtype file.",DefineConstants.STOP);
		}

		if (fscanc(infile, "%d", initialLastFire) != 1)
		{

			errorSys("Error reading in initialLastFire from landtype file.",DefineConstants.STOP);
		}

		for (i = 0;i < 5;i++)
		{

			if (fscanc(infile, "%d", fireCurve[i]) != 1)
			{

				errorSys("Error reading in fireCurve from landtype file.",DefineConstants.STOP);
			}
		}

		for (i = 0;i < 5;i++)
		{

			if (fscanc(infile, "%d", fireClass[i]) != 1)
			{

				errorSys("Error reading in fireClass from landtype file.",DefineConstants.STOP);
			}
		}

		for (i = 0;i < 5;i++)
		{

			if (fscanc(infile, "%d", windCurve[i]) != 1)
			{

				errorSys("Error reading in fireCurve from landtype file.",DefineConstants.STOP);
			}
		}

		for (i = 0;i < 5;i++)
		{

			if (fscanc(infile, "%d", windClass[i]) != 1)
			{

				errorSys("Error reading in fireClass from landtype file.",DefineConstants.STOP);
			}
		}





		if (_stricmp(name,"empty") == 0)
		{

			status = DefineConstants.PASSIVE;
		}

		else if (_stricmp(name,"water") == 0)
		{

			status = DefineConstants.WATER;
		}

		else if (_stricmp(name,"wetland") == 0)
		{

			status = DefineConstants.WETLAND;
		}

		else if (_stricmp(name,"bog") == 0)
		{

			status = DefineConstants.BOG;
		}

		else if (_stricmp(name,"lowland") == 0)
		{

			status = DefineConstants.LOWLAND;
		}

		else if (_stricmp(name,"nonforest") == 0)
		{

			status = DefineConstants.NONFOREST;
		}

		else
		{

			status = DefineConstants.ACTIVE;
		}



	}

	public void write(FILE outfile)

	{

		int i;

		LDfprintf0(outfile,"%d \n",fireInterval);

		LDfprintf0(outfile,"%f \n",m_fIgPoisson);

		LDfprintf0(outfile,"%f \n",m_fMFS);

		LDfprintf0(outfile,"%f \n",m_fFireSTD);

		for (i = 0;i < 5;i++)
		{

			LDfprintf0(outfile,"%d ",fireCurve[i]);
		}

		LDfprintf0(outfile,"\n");

		for (i = 0;i < 5;i++)
		{

			LDfprintf0(outfile,"%d ",fireClass[i]);
		}

		LDfprintf0(outfile,"\n");

	}

	public void dump()

	{

		int i;

		Console.Write("Name:          {0}\n",name);

		Console.Write("fireInterval:  {0:D}\n",fireInterval);

		Console.Write("IgnitionPoissonParameter:  {0:f}\n",m_fIgPoisson);

		Console.Write("MFS:      {0:f}\n",m_fMFS);

		Console.Write("FS STD deviation:      {0:f}\n",m_fFireSTD);

		for (i = 0;i < 5;i++) //Nim: changed int i to i
		{

			Console.Write("Class {0:D}:  {1:D}\n",fireClass[i],fireCurve[i]);
		}

	}





	public int active()

	{

	if (status == DefineConstants.ACTIVE)
	{

		return true;
	}

	else
	{

		return false;
	}

	}

						   //Inactive fire regime units are not processed.

	public int water()

	{

		if (status == DefineConstants.WATER)
		{

			return true;
		}

		else
		{

			return false;
		}

	}

						   //otherwise.	




	private int status; //Either ACTIVE, PASSIVE, or WATER.



}



