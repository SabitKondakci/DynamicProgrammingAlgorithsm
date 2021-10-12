
void Main()
{
	GenerateLicencePlate(10); // first creation with Shared.Return
	SharedMemory(10); // rent already created array of WisconsinDriverLicenseInfo
}

public void SharedMemory(int nItemAtATime)
{
	WisconsinDLPool wisPoolObject = new();
	var pooledList = wisPoolObject.Rent(nItemAtATime); // WisconsinDriverLicenseInfo[]
	pooledList.Dump();
}

#region Enum_Hair&Eye_Color
public enum HairColor : byte
{
	None,
	Black,
	Yellow,
	White,
	Red,
	Brown,
	PaintedMi
}

public enum EyeColor : byte
{
	None,
	Black,
	Blue,
	Green,
	Brown,
	Brown_Amber,
	Gray,
	Hazel
}
#endregion

sealed class SingletonArrayPool<T>
{
	private static readonly Lazy<ArrayPool<T>> _singleton =
	new Lazy<ArrayPool<T>>(() => ArrayPool<T>.Shared);
	private SingletonArrayPool() { }
	public static ArrayPool<T> Shared => _singleton.Value;
}

public record WisconsinDriverLicenseInfo
{
	public string licenceId { get; set; }
	public char licenceClass { get; set; }
	public char sex { get; set; }
	public string address { get; set; }
	public ushort weightPound { get; set; }
	public ushort heightInch { get; set; }
	public DateTime dateOfBirth { get; set; }
	public DateTime expirationDate { get; set; }
	public HairColor hairColor { get; set; }
	public EyeColor eyeColor { get; set; }
	public bool IsDonor { get; set; }
	public DateTime ISS { get; set; }
}

public void GenerateLicencePlate(int nItemAtATime)
{
	HairColor[] hairColors = Enum.GetValues<HairColor>();
	EyeColor[] eyeColors = Enum.GetValues<EyeColor>();
	Random randomHairColor = new();
	Random randomEyeColor = new();

	WisconsinDLPool wisPoolObject = new();
	var wisconsinDLInfoArray = wisPoolObject.Rent(nItemAtATime);

	for (int i = 0; i < nItemAtATime; i++)
	{
		int randomHCNext = randomHairColor.Next(1, hairColors.Length);
		int randomECNext = randomEyeColor.Next(1, eyeColors.Length);

		wisconsinDLInfoArray[i] =
		new WisconsinDriverLicenseInfo()
		{
			licenceId = "licenseId",
			licenceClass = '#',
			sex = '#',
			address = "address",
			weightPound = 1,
			heightInch = 1,
			dateOfBirth = DateTime.UtcNow,
			expirationDate = DateTime.UtcNow,
			hairColor = hairColors[randomHCNext],
			eyeColor = eyeColors[randomECNext],
			IsDonor = true,
			ISS = DateTime.UtcNow
		};
	}

	wisconsinDLInfoArray.Dump();
	wisPoolObject.Return(wisconsinDLInfoArray, true);
}

public class WisconsinDLPool : ArrayPool<WisconsinDriverLicenseInfo>
{

	private int length;

	public override WisconsinDriverLicenseInfo[] Rent(int minimumLength)
	{
		length = minimumLength;
		return SingletonArrayPool<WisconsinDriverLicenseInfo>.
		Shared.Rent(minimumLength);
	}


	public override void Return(WisconsinDriverLicenseInfo[] array,
	bool clearArray = false)
	{
		if (clearArray)
		{
			for (int i = 0; i < length; i++)
			{
				array[i] =
				new WisconsinDriverLicenseInfo()
				{
					licenceId = "#",
					licenceClass = '#',
					sex = '#',
					address = "#",
					weightPound = 0,
					heightInch = 0,
					dateOfBirth = DateTime.MinValue,
					expirationDate = DateTime.MaxValue,
					hairColor = HairColor.None,
					eyeColor = EyeColor.None,
					IsDonor = false,
					ISS = DateTime.MinValue
				};
			}

		}

		SingletonArrayPool<WisconsinDriverLicenseInfo>.
		Shared.Return(array);
	}
}

