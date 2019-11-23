#pragma once
namespace ManagedCPP_DLL 
{

	template<class T>
	public ref class ManagedObject
	{
	protected:
		T* _Instance;
	public:
		ManagedObject(T* instance)
			: _Instance(instance)
		{
		}

		// Destructor to clear after use.
		virtual ~ManagedObject()
		{
			if (_Instance != nullptr)
			{
				delete _Instance;
			}
		}

		// Finalizer for garbage colletor purpose.
		!ManagedObject()
		{
			if (_Instance != nullptr)
			{
				delete _Instance;
			}
		}
		
		// Getter to get instance of actual menaged object.
		T* GetInstance()
		{
			return _Instance;
		}

		// TODO: Add any conversion functions here to call it.
		// I.e int array to pointer refered to first element of this array converter.
		static void int_array_conversion(array<int>^ data)
		{
			pin_ptr<unsigned int> arrayPin = &data[0];
			unsigned int size = data->Length;
		}
	};
}