//Originally from AssetFactory
namespace TakoStories.IO
{
	public interface IByteConvertible
	{
		void FromBytes(byte[] bytes);
		byte[] ToBytes();
	}
}