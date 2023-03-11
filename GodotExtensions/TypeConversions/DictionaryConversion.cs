using System;
using System.Collections.Generic;
using System.Linq;

namespace GodotExtensions.TypeConversions
{
    public static class DictionaryConversion
    {
        public static Dictionary<TKey, TValue> ToDictionaryNative<TKey, TValue>(this Godot.Collections.Dictionary gdDict)
            where TKey : IComparable,IConvertible,IEquatable<TKey> //native type
            where TValue : IComparable,IConvertible,IEquatable<TValue> //native type
        {
            IEnumerable<TKey> keys = gdDict.Keys.Cast<TKey>();
            IEnumerable<TValue> values = gdDict.Values.Cast<TValue>();
            return keys.Zip(values, (k, v) => new {k, v}).ToDictionary(x => x.k, x=> x.v);
        }
        
        //todo: pull out logic to apply to ICollection/IEnumerable/Iafpwenfasdf 
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this Godot.Collections.Dictionary gdDict) 
            where TKey : IComparable,IConvertible,IEquatable<TKey> //native type
            where TValue : IGodotConversionBase, new() 
        {
            IEnumerable<TKey> keys = gdDict.Keys.Cast<TKey>();
            IEnumerable<TValue> values = gdDict.Values
                .Cast<Godot.Object>()
                .Select(x =>
                {
                    var nu = new TValue();
                    nu.SetValuesFromObject(x);
                    return nu;
                });
            
            return keys.Zip(values, (k, v) => new {k, v}).ToDictionary(x => x.k, x=> x.v);
        }
    }
}