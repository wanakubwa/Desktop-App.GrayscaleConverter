#pragma once
namespace GrayscaleCppManager 
{
	using namespace System::Runtime::InteropServices;
	using namespace System;
	/// Class using for wraping all cpp cores into one class included destructor and garbage collector finalizer.
	/// This is tamplate can contain any class from "GrayscaleCppCore".
	/// using also to connect c# data to cpp data t.m conatins conversion static methods.
	template<class T>
	public ref class ManagedObject
	{
	protected:
		/// Instance of called in c# object.
		T* _Instance;
	public:
		ManagedObject(T* instance)
			: _Instance(instance)
		{
		}

		/// Destructor to clear after use.
		virtual ~ManagedObject()
		{
			if (_Instance != nullptr)
			{
				delete _Instance;
			}
		}

		/// Finalizer for garbage colletor purpose.
		!ManagedObject()
		{
			if (_Instance != nullptr)
			{
				delete _Instance;
			}
		}
		
		/// Getter to get instance of actual menaged object.
		T* GetInstance()
		{
			return _Instance;
		}

		// TODO: Add any conversion functions here to call it.
		// I.e int array to pointer refered to first element of this array converter.

		// Converting array of bytes (1 byte == 8bit) to pointer to first element of array (unsigned char == 8bit).
		// Return pointer.
		static unsigned char* byte_array_conversion(array<Byte>^ data)
		{
			pin_ptr<unsigned char> arrayPin = &data[0];
			unsigned char size = data->Length;
			return arrayPin;
		}

		// Testing conversion. Not in use but can be helpfull in future.
		static char* string_to_char_array(String^ string)
		{
			char* str = (char*)(Marshal::StringToHGlobalAnsi(string)).ToPointer();
			return str;
		}
	};
}