// FireCell.cpp: implementation of the CFinneyCell class.

//

//////////////////////////////////////////////////////////////////////





// FinneyCell.h: interface for the CFinneyCell class.

//

//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000


#endif



public class CFinneyCell : System.IDisposable

{


	public void setValue(int r, int c, float time)

	{

		row = r;

		col = c;

		minTime = time;

	}

	public float minTime;

	public int col;

	public int row;


	//////////////////////////////////////////////////////////////////////

	// Construction/Destruction

	//////////////////////////////////////////////////////////////////////



	public CFinneyCell()

	{



	}

	public virtual void Dispose()

	{



	}

	public static bool operator < (CFinneyCell ImpliedObject, CFinneyCell right)

	{



		return (ImpliedObject.minTime < right.minTime);

	}

	public static bool operator > (CFinneyCell ImpliedObject, CFinneyCell right)

	{



		return (ImpliedObject.minTime > right.minTime);

	}


	public static bool operator == (CFinneyCell ImpliedObject, CFinneyCell right)

	{



		return (ImpliedObject.minTime == right.minTime);

	}

	public static bool operator != (CFinneyCell ImpliedObject, CFinneyCell right)

	{



		return (ImpliedObject.minTime != right.minTime);

	}



}