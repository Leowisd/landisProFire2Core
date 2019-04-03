using System;

// FireRegimeUnits.cpp: implementation of the CFireRegimeUnits class.

//

//////////////////////////////////////////////////////////////////////



// FireRegimeUnits.h: interface for the CFireRegimeUnits class.

//

//////////////////////////////////////////////////////////////////////


#if _MSC_VER > 1000


#endif


public class CFireRegimeUnits : System.IDisposable

{


	public void updateIGDensity(int cellSize)

	{



		for (int i = 0; i < numLU; i++)

		{

			pFireRegimeUnit[i].m_fIgPoisson = pFireRegimeUnit[i].m_fIgPoisson * NumOfSites[i] * cellSize * cellSize / 10000;

		}





	}

	public int[] NumOfSites = new int[256];



	//CFireRegimeUnits(int n=MAX_LANDUNITS,StochasticLib * pStochastic=NULL);	


	//////////////////////////////////////////////////////////////////////

	// Construction/Destruction

	//////////////////////////////////////////////////////////////////////



	public CFireRegimeUnits(int n, StochasticLib pStochastic)

	{

		numLU = 0;

		pFireRegimeUnit = Arrays.InitializeWithDefaultInstances<CFireRegimeUnit>(n);



		currentLU = 0;

		maxLU = n;

		m_pStochastic = pStochastic;

		for (int i = 0; i < 256; i++)

		{

			NumOfSites[i] = 0;

		}

	}

	public virtual void Dispose()

	{

		if (pFireRegimeUnit != null)
		{

			Arrays.DeleteArray(pFireRegimeUnit);
		}

	}



	//sitesWRTFRUList * sitesWRTFRUListArray; // sites with respect to fire regime unit



	public void read(FILE infile)

	//Read set of Fire regime units from a file.

	{

		numLU = 0;

		while (!LDeof(infile))

		{

			if (numLU < maxLU) //J.Yang 04/16/2002

			{

				pFireRegimeUnit[numLU].read(infile);

				pFireRegimeUnit[numLU].index = numLU;

				numLU++;



			}

			else
			{

				errorSys("CFireRegimeUnits::read(FILE*)-> Array bounds error.",DefineConstants.STOP);
			}

		}

	}

	public void write(FILE outfile)

	//Write set of land units to a file.



	{

		for (int i = 0;i < numLU;i++)

		{

			pFireRegimeUnit[i].write(outfile);

		}

	}

	public void dump()

	//Dump set of land units to the CRT.



	{

		for (int i = 0;i < numLU;i++)

		{

			pFireRegimeUnit[i].dump();

			Console.Write("===================================\n");

		}

	}

	public void dispatch()

	{

		int i;
		int j;
		int k;

		int snr;
		int snc;





		snr = m_pFireSites.numRows();

		snc = m_pFireSites.numColumns();

		for (k = 0; k < numLU; k++)
		{

			NumOfSites[k] = 0;
		}



		for (i = 1;i <= snr;i++)
		{

			for (j = 1;j <= snc;j++)

			{



				k =  m_pFireSites.functorMethod(i,j).FRUIndex;

				NumOfSites[k]++;



			}
		}

	}

							  //sitesWRTlandunitListPtr[i]



	public static CFireRegimeUnit functorMethod(ref string name)

	//Referrence a fire regime unit by fire regime unit name.



	{

	for (int i = 0;i < numLU;i++)
	{

		if (string.Compare(name,pFireRegimeUnit[i].name) == 0)
		{

		   return pFireRegimeUnit[i];
		}
	}



	return null;

	}

	public static CFireRegimeUnit functorMethod(int n)

	//Referrence a land unit by land unit number.



	{

		if (n > numLU || n < 0)
		{

			return null;
		}

		else
		{

			return pFireRegimeUnit[n]; // be careful this pointer. J.Yang
		}

	}

	public CFireRegimeUnit first()

	//Referrence first fire regime unit.



	{

		currentLU = 0;

		if (numLU == 0)
		{

			return null;
		}

		else
		{

			return pFireRegimeUnit[0];
		}

	}

	public CFireRegimeUnit next()

	//Referrence next fire regime unit.



	{

		currentLU++;

		if (currentLU >= numLU)
		{

			return null;
		}

		else
		{

			return pFireRegimeUnit[currentLU];
		}

	}

									 //NOTE: All four referrence functions return 

							  //NULL if attribute referrenced is illeagal or

							  //unavailable.

	public int number()

	//Returns number of fire regime units.



	{

		return numLU;

	}

	public void attach(CFireSites pFireSites)

	{

		m_pFireSites = pFireSites.functorMethod;

	}

	public void readFireRegimeGIS(FILE mapFile)

	{

	//This will read fire regime map and associate fire regime Unit to each site.

	//mapFile is an Erdas 8 bit gis file.  The file pointer is

	//placed on the first map element.  yDim and xDim are the (x,y) dimensions

	//of the Erdas map.



		byte c;

		uint[] dest = new uint[64];

		int nCols;
		int nRows;
		int numRead;
		int coverType;



		FIRESITE s;



		int b16or8; //true: 16, false 8 bit

		ushort intdata;





		LDfread((string)dest,4,32,mapFile);

		if ((dest[1] & 0xff0000) == 0x020000)
		{

			b16or8 = 16;
		}

		else if ((dest[1] & 0xff0000) == 0)
		{

			b16or8 = 8;
		}

		else

		{

			b16or8 = -1;

			errorSys("Error: IO: Fire regime GIS map is niether 16 bit or 8 bit.", DefineConstants.STOP);

		}

#if __UNIX__

		ERDi4_c(dest[4], nCols);

		ERDi4_c(dest[5], nRows);

#else

		nCols = dest[4];

		nRows = dest[5];

#endif

		if ((nCols != m_pFireSites.m_iCols) || (nRows != m_pFireSites.m_iRows))
		{

			errorSys("the dimension of fire regime GIS map is not consistent.",DefineConstants.STOP);
		}







		if (b16or8 == 8) //8 bit

		{

			for (int i = nRows;i > 0;i--)

			{

				for (int j = 1;j <= nCols;j++)

				{

					numRead = LDfread((string)(c), 1, 1, mapFile);

					coverType = (int)c;

					if ((numRead >= 0) && (coverType >= 0))

					{

						//s=sites(i,j);

						//s->landUnit=landUnits(coverType);

						//<Add By Qia on Nov 24 2008>

						m_pFireSites.BefStChg(i, j);

						//</Add By Qia on Nov 24 2008>

						s = m_pFireSites.operator ()(i,j); //fire site

						s.FRUIndex = coverType;

						//<Add By Qia on Nov 24 2008>

					m_pFireSites.AftStChg(i, j);

					//</Add By Qia on Nov 24 2008>

					}

					else
					{

						errorSys("illegal landtype class found7.",DefineConstants.STOP);
					}

				}

			}

		}

		else if (b16or8 == 16) //16 bit

		{

			for (int i = nRows;i > 0;i--)

			{

				for (int j = 1;j <= nCols;j++)

				{

					numRead = LDfread((string)(intdata), 2, 1, mapFile);

					coverType = (int)intdata;

					if ((numRead >= 0) && (coverType >= 0))

					{

						//<Add By Qia on Nov 24 2008>

					m_pFireSites.BefStChg(i, j);

					//</Add By Qia on Nov 24 2008>

						s = m_pFireSites.operator ()(i,j); //fire site

						s.FRUIndex = coverType;

						//<Add By Qia on Nov 24 2008>

					m_pFireSites.AftStChg(i, j);

					//</Add By Qia on Nov 24 2008>

					}

					else
					{

						errorSys("illegal landtype class found8.",DefineConstants.STOP);
					}

				}

			}

		}







	}

	public void readFireRegimeIMG(GDALDataset fpImg)

	{

		//This will read fire regime .img map and associate fire regime Unit to each site.

		int nCols;
		int nRows;
		int numRead;
		int coverType;
		int noDataValue;

		FIRESITE s;

		//unsigned short		 intdata;

		float pafScanline; //*

		GDALRasterBand poBand; //*

		poBand = fpImg.GetRasterBand(1); //*

		//LDfread((char*)dest, 4, 32, mapFile);


#if __UNIX__

		ERDi4_c(dest[4], nCols);

		ERDi4_c(dest[5], nRows);

#else

		nCols = fpImg.GetRasterXSize(); //*

		nRows = fpImg.GetRasterYSize(); //*

#endif

		if ((nCols != m_pFireSites.m_iCols) || (nRows != m_pFireSites.m_iRows))
		{

			errorSys("the dimension of fire regime GIS map is not consistent.", DefineConstants.STOP);
		}

		pafScanline = (float)CPLMalloc(sizeof(float) * (nCols * nRows));

		poBand = fpImg.GetRasterBand(1);

		poBand.RasterIO(GF_Read, 0, 0, nCols, nRows, pafScanline, nCols, nRows, GDT_Float32, 0, 0);

		noDataValue = GDALGetRasterNoDataValue(poBand, null);

			for (int i = nRows; i > 0; i--)
			{
				for (int j = 1; j <= nCols; j++)
				{
					coverType = (int) * (pafScanline + (nRows - i) * nCols + j - 1); //*

					if (coverType == noDataValue)
					{
						coverType = 0;
					}
					if (coverType >= 0)
					{
						m_pFireSites.BefStChg(i, j);

						s = m_pFireSites.operator ()(i, j); //fire site

						s.FRUIndex = coverType;

						m_pFireSites.AftStChg(i, j);

					}
					else
					{

						errorSys("Illegal fire site class found9.", DefineConstants.STOP);
					}

				}

			}

	}




	private CFireRegimeUnit[] pFireRegimeUnit; //Array holding all fire regime units.

	private CFireSites m_pFireSites.functorMethod;

	private StochasticLib m_pStochastic;

	private int numLU; //Number of fire regime units.

	private int currentLU; //Current fire regime unit being pointed to by first

								  //and next access functions.

	private int maxLU; //Maximum number of fire regime units.  Defined upon

								  //class construction.




}