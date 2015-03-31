namespace Spam
{
	public interface IChangeNotifier<K>
	{
		void SetChanged(Entity<K> entity);
	}
}