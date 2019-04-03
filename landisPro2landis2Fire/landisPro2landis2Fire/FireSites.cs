using System;
using System.Collections.Generic;

/********************************************

Name:			FireSites.cpp 

Description:	FireSites

Input:			

Output:			

Date:			Feb. 18, 2004

Last Mod:		Feb. 18

*******************************************/

/********************************************

Name:			FireSites.h 

Description:	FireSites

Input:			

Output:			

Date:			Feb. 18, 2004

Last Mod:		Feb. 18

*******************************************/









//#include "FireRegimeUnits.h"



//<Add By Qia on Nov 24 2008>


//</Add By Qia on Nov 24 2008>







public class FIRESITE

{

	public int FRUIndex;

	public int DEM; //elevation, data type should be discussed, J.Yang

	public int numofsites; //Add By Qia on Nov 24 2008

}



//<Add By Qia on Nov 24 2008>


//</Add By Qia on Nov 24 2008>



public class CFireSites : System.IDisposable

{




	public int m_iRows;

	public int m_iCols;

	//<Add By Qia on Nov 24 2008>

	public List<FIRESITE > SortedIndex = new List<FIRESITE >();

	//</Add By Qia on Nov 24 2008>

	//FIRESITE*	m_pFireLAND; //commented by Qia on Nov 24 2008




	//<Add By Qia on Nov 24 2008>



	//</Add By Qia on Nov 24 2008>





	public CFireSites()

	{

		m_iRows = 0;

		m_iCols = 0;

		//m_pFireLAND = NULL;//commented By Qia on Nov 24 2008

	}

	public CFireSites(int i, int j)

	{

		m_iRows = i;

		m_iCols = j;

		//m_pFireLAND = new FIRESITE[i*j]; //commented By Qia on Nov 24 2008

		map = new FIRESITE[i * j]; //Add By Qia on Nov 24 2008

		Console.Write("LandisPro fire map allocated\n");

		FIRESITE temp;

		int x;

		sitetouse = new FIRESITE();

		temp = new FIRESITE();

		temp.FRUIndex = 0;

		temp.DEM = 0;

		temp.numofsites = i * j;

		SortedIndex.Add(temp);

		for (i = 1; i <= m_iRows; i++)
		{

			for (j = 1; j <= m_iCols; j++)
			{

				//this->operator ()(i,j)->DEM = 0;

				x = (i - 1) * m_iCols;

				x = x + j - 1;

				map[x] = temp;

			}
		}



	}

	public void Dispose()

	{

		//if (m_pFireLAND)

			//delete [] m_pFireLAND;  //commented By Qia on Nov 24 2008

		if (map != null)
		{
			Arrays.DeleteArray(map); //Add By Qia on Nov 24 2008
		}

		//<Add By Qia on Nov 24 2008>

		for (int i = 0;i < SortedIndex.Count;i++)

		{

			FIRESITE temp;

			temp = SortedIndex[i];

			if (temp != null)
			{
				temp.Dispose();
			}

		}

		//</Add By Qia on Nov 24 2008>

		if (sitetouse != null)
		{
			sitetouse.Dispose();
		}

	}

	public static FIRESITE functorMethod(int i, int j)

	{

		int x;



		if (i <= 0 || i> m_iRows || j <= 0 || j> m_iCols)

		{

			string err = new string(new char[80]);

			err = string.Format("CFireSites::operator() (int,int)-> ({0:D}, {1:D}) are illegal map                  coordinates", i, j);

			errorSys(err,DefineConstants.STOP);

		}



		x = (i - 1) * m_iCols;

		x = x + j - 1;

		return map[x];

	}



	//void	readFireRegime(FILE*,CFireRegimeUnits*);	

	//void	write		(FILE*);	

	//void	dump		();	



	public int number()

	//This will return the total number of sites.



	{

		return numRows() * numColumns();

	}

	public int numRows()

	{

		return m_iRows;

	}

	public int numColumns()

	{

		return m_iCols;

	}

	//<Add by Qia on Nov 24 2008>
	//<Add By Qia on Dec 26 2013>
	public List<LDPOINT>[] FireRegimeUnitsList = Arrays.InitializeWithDefaultInstances<List>(70000);
	public int[] FireRegimeCurrentIndex = new int[70000];
	public void create_FireRegimeUnitsListByIndex()
	{
		int i = 0;
		int j = 0;
		int x;
		int max_FRUIndex = 0;
		for (i = 1;i <= m_iRows;i++)
		{
			for (j = 1;j <= m_iCols;j++)
			{
				x = (i - 1) * m_iCols;
				x = x + j - 1;
				LDPOINT p = new LDPOINT(j, i);
				FireRegimeUnitsList[map[x].FRUIndex].Add(p);
			}
		}
		for (i = 0;i < 70000;i++)
		{
			FireRegimeCurrentIndex[i] = 0;
		}
	}
	public LDPOINT return_a_point_from_index(int index, int cellNo)
	{
		if (cellNo > FireRegimeUnitsList[index].Count)
		{
			Console.Write("Fire index error -");
			Console.Write(index);
			Console.Write("\n");
			errorSys("Fire index error, contact development team\n",DefineConstants.STOP);
		}
		cellNo--;
		LDPOINT p = FireRegimeUnitsList[index][cellNo];
		List<LDPOINT>.Enumerator sitePtr;
		sitePtr = FireRegimeUnitsList[index].GetEnumerator();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
		FireRegimeUnitsList[index].erase(sitePtr + cellNo);

		FireRegimeCurrentIndex[index] = (FireRegimeCurrentIndex[index] + 1) % 70000;
		return p;
	}
	//</Add By Qia on Dec 26 2013>
public int SITE_compare(FIRESITE site1, FIRESITE site2)

{

	if (site1.FRUIndex > site2.FRUIndex)
	{

		return 1;
	}

	if (site1.FRUIndex < site2.FRUIndex)
	{

		return 2;
	}

	if (site1.DEM > site2.DEM)
	{

		return 1;
	}

	if (site1.DEM < site2.DEM)
	{

		return 2;
	}

	return 0;

}


//<Add by Qia on Nov 24 2008>

public int SITE_compare(int site1_x, int site1_y, int site2_x, int site2_y)

{

	int x;
	int y;

	int result;

	FIRESITE site1;
	FIRESITE site2;

	x = (site1_x - 1) * m_iCols;

	x = x + site1_y - 1;



	y = (site2_x - 1) * m_iCols;

	y = y + site2_y - 1;



	site1 = map[x];

	site2 = map[y];

	result = SITE_compare(site1, site2);

	return result;

}

								   // Compare two sites to see the relation between them

								   // return 0:equal; return 1: site1 is bigger; return 2: site2 is bigger; -1: error



public int SITE_sort()

//use babble algorithm to sort the initial site list array

{

	int size;
	int i;
	int j;

	FIRESITE site1;
	FIRESITE site2;
	FIRESITE temp;

	size = SortedIndex.Count;

	for (i = SortedIndex.Count - 1;i > 0;i--)

	{

			for (j = 0;j <= i - 1;j++)

			{

					site1 = SortedIndex[j];

					site2 = SortedIndex[j + 1];

					if (SITE_compare(site1, site2) == 1)

					{

							temp = SortedIndex[j];

							SortedIndex[j] = SortedIndex[j + 1];

							SortedIndex[j + 1] = temp;

					}

			}

	}

	return 1;

}



public int SITE_insert(int pos_sortIndex, FIRESITE site, int i, int j)

//when there is a new site during succession or whatever, we need to

//check if the new site already exists, if yes combine with existing one

//if not insert to the position according to sort

{



	int x;

	int ifexist = 0;
	int pos = 0;

	FIRESITE temp;

	x = (i - 1) * m_iCols;

	x = x + j - 1;

	List<FIRESITE >.Enumerator temp_sitePtr;

	temp_sitePtr = SortedIndex.GetEnumerator();



	SITE_LocateinSortIndex(site, ref pos, ref ifexist);

	if (ifexist != 0)

	{

			map[x] = SortedIndex[pos];

			map[x].numofsites++;

			//delete  site;

	}

	else

	{
			temp = new FIRESITE();

			temp.FRUIndex = site.FRUIndex;

	temp.DEM = site.DEM;

			temp.numofsites = 1;

			map[x] = temp;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
			SortedIndex.insert(temp_sitePtr + pos, temp);

	}

	return 1;

}



public int SITE_delete(int pos_sortIndex, FIRESITE site, int i, int j)

//When a site disappears, delete it

{

	int x;

	List<FIRESITE >.Enumerator temp_sitePtr;

	x = (i - 1) * m_iCols;

	x = x + j - 1;

	if (site != SortedIndex[pos_sortIndex])
	{

		return 0;
	}

	if (site != map[x])
	{

		return 0;
	}

	site = null;

	temp_sitePtr = SortedIndex.GetEnumerator();

	SortedIndex.erase(temp_sitePtr + pos_sortIndex);

	return 1;

}





public int SITE_LocateinSortIndex(FIRESITE site, ref int pos, ref int ifexist)

//Find if a new site exists in sorted list

//If a new site exists, find its location and set *ifexist as 1

//if this no site matches this one, find location before which new site pointer should be inserted

//By Qia Oct 09 2008

{

	int begin;
	int end;
	int mid;

	FIRESITE temp;

	int temp_flag;

	ifexist = 0;

	begin = 0;

	end = SortedIndex.Count;

	if (end == 0)
	{

		Console.Write("No site at all wrong wrong wrong\n");

		return -1;

	}

	end--;

	mid = (begin + end) / 2;

	temp = SortedIndex[mid];

	while (begin < end)

	{

			temp_flag = SITE_compare(site, temp);

			if (temp_flag == 0)

			{

					ifexist = 1;

					pos = mid;

					return 1;

			}

			else if (temp_flag == 1)

			{

					begin = mid + 1;

					mid = (begin + end) / 2;

			}

			else if (temp_flag == 2)

			{

					end = mid - 1;

					mid = (begin + end) / 2;

			}

			else

			{

					return -1;

			}

			temp = SortedIndex[mid];

	}

	temp_flag = SITE_compare(site, temp);

	if (temp_flag == 0)

	{

			ifexist = 1;

			pos = mid;

			return 1;

	}

	else if (temp_flag == 2)

	{

			ifexist = 0;

			pos = mid;

			return 0;

	}

	else if (temp_flag == 1)

	{

			ifexist = 0;

			pos = mid + 1;

			return 0;

	}

	else

	{

			return -1;

	}

}










//</Add by Qia on Oct 08 2008>



//<Add by Qia on Oct 08 2008>

public void fillinSitePt(int i, int j, FIRESITE site)

//

{

	int x;

	x = (i - 1) * m_iCols;

	x = x + j - 1;

	map[x] = site;

}

public FIRESITE locateSitePt(int i, int j)

//

{

	int x;

	x = (i - 1) * m_iCols;

	x = x + j - 1;

	return map[x];

}









public void BefStChg(int i, int j)

//Before Site Change

//This function back up a site and following changes are based on this seprated site

//sort vector is not touched here

{

	FIRESITE temp;

	temp = locateSitePt(i, j);

	sitetouse.FRUIndex = temp.FRUIndex;

	sitetouse.DEM = temp.DEM;



	if (temp.numofsites == 1)

	{

			int pos;
			int ifexist = 0;

			SITE_LocateinSortIndex(sitetouse, ref pos, ref ifexist);

			if (ifexist != 0)

			{

					List<FIRESITE >.Enumerator temp_sitePtr;

					temp_sitePtr = SortedIndex.GetEnumerator();

					SortedIndex.erase(temp_sitePtr + pos);

					temp = null;

			}

			else

			{

					Console.Write("num of vectors {0:D}\n",SortedIndex.Count);

					Console.Write("ERROR ERROR ERROR ERROR!!~~~{0:D}\n",pos);



			}

	}

	else if (temp.numofsites <= 0)

	{

			Console.Write("FIRESITE NO NO NO NO NO\n");

	}

	else

	{

			temp.numofsites--;

	}

	fillinSitePt(i, j, sitetouse);

	return;

}

public void AftStChg(int i, int j)

//After Site Change

//This function does combination and delete of the seprated site made by BefStChg(int i, int j)

//insert this site to the sorted vector

{

	SITE_insert(0, sitetouse, i, j);

	return;

}

//</Add By Qia on Nov 24 2008>



//<Add By Qia on Nov 24 2008>




private FIRESITE[] map;

private FIRESITE sitetouse; //Add By Qia on Dec 19 2008



//</Add By Qia on Nov 24 2008>





}

//</Add By Qia on Nov 24 2008>





