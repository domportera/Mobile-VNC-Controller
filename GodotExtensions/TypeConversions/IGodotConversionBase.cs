using Godot;

namespace GodotExtensions.TypeConversions
{
   public interface IGodotConversionBase
   {
      void SetValuesFromObject(GodotObject obj);
   }
}