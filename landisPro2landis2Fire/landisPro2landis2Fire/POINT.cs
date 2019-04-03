//#ifdef __HARVEST__



//#endif

//



public class LDPOINT : System.IDisposable
{



	//#endif








	// Also change "struct POINT" to "POINT"



	public LDPOINT()
	{

		x = y = 0;

	}

	public LDPOINT(int tx, int ty)
	{
		this.x = tx;
		this.y = ty;
	}

	public void Dispose()
	{
	}

	public static int operator == (LDPOINT ImpliedObject, LDPOINT right)
	{

		return (ImpliedObject.x == right.x) && (ImpliedObject.y == right.y);

	}

//#ifdef __HARVEST__

#if __HARVEST__
	public void print(ostream os)
	{

		os << "{" << x << "," << y << "}";

	}

//#endif

	public int x;
	public int y;

}