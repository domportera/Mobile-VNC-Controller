using System;
using Object = Godot.Object;

namespace GodotExtensions.TypeConversions
{
   public interface IGodotConversionBase
   {
      void SetValuesFromObject(Godot.Object obj);
   }
}