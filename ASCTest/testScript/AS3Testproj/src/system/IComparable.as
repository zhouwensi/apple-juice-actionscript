package system 
{
	
	[link_system_interface(system_icomparable_creator_)]
	public interface IComparable 
	{
		[native,_system_icomparable_compareto_]
		function compareTo(obj:_Object_):int;
	}
	
}