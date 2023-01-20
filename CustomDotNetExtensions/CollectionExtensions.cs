using System;
using System.Collections.Generic;

public static class CollectionExtensions
{
    /// <summary>
    /// An optimized way to call foreach on a list. Uses a normal for loop.
    /// Note that just like a regular foreach loop, you cannot modify this collection from within your action,
    /// i.e. by assigning a new instance or value to your action's parameter.
    /// This function won't warn you about it, but you may get exceptions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="perItemAction">Action to be called on each item in the list</param>
    public static void ForEach<T>(this List<T> list, Action<T> perItemAction) where T : class
    {
        for(int i = 0; i < list.Count; i++)
        {
            perItemAction(list[i]);
        }
    }

    /// <summary>
    /// An optimized way to call foreach on a list. Uses a normal for loop.
    /// Note that just like a regular foreach loop, you cannot modify this collection from within your action,
    /// i.e. by assigning a new instance or value to your action's parameter, or by modifying this list manually using the provided index.
    /// This function won't warn you about it, but you may get exceptions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="perItemActionWithIndex">Action to be called on each item in the list. The integer parameter is the index in the for loop.</param>
    public static void ForEach<T>(this List<T> list, Action<T, int> perItemActionWithIndex) where T : class
    {
        for (int i = 0; i < list.Count; i++)
        {
            perItemActionWithIndex(list[i], i);
        }
    }

    /// <summary>
    /// Make two elements of this list swap places
    /// </summary>
    /// <param name="itemToSwap1">an item currently in this list</param>
    /// <param name="itemToSwap2">an item currently in this list</param>
    public static void Swap<T>(this List<T> list, T itemToSwap1, T itemToSwap2)
    {
        int firstIndex = list.IndexOf(itemToSwap1);
        int secondIndex = list.IndexOf(itemToSwap2);

        bool missingFirst = firstIndex == -1;
        bool missingSecond = secondIndex == -1;
        bool missingBoth = missingFirst && missingSecond;

        if(missingBoth)
        {
            throw new ArgumentException(GenerateSwapFailedString(itemToSwap1, itemToSwap2));
        }

        if(missingFirst)
        {
            throw new ArgumentException(GenerateMoveFailedString(itemToSwap1));
        }

        if(missingSecond)
        {
            throw new ArgumentException(GenerateMoveFailedString(itemToSwap2));
        }

        list[firstIndex] = itemToSwap2;
        list[secondIndex] = itemToSwap1;
    }

    /// <summary>
    /// Moves provided element of this list to the end of this list (index = Count - 1)
    /// </summary>
    /// <param name="element">An item currently in this list</param>
    public static void MoveItemToEnd<T>(this List<T> list, T element)
    {
        list.MoveItemTo(element, list.Count - 1);
    }

    /// <summary>
    /// Moves provided element of this list to the start of this list (index = 0)
    /// </summary>
    /// <param name="element">An item currently in this list</param>
    public static void MoveItemToFront<T>(this List<T> list, T element)
    {
        list.MoveItemTo(element, 0);
    }

    /// <summary>
    /// Moves provided element of this list to the provided index
    /// </summary>
    /// <param name="element">An item currently in this list</param>
    public static void MoveItemTo<T>(this List<T> list, T element, int newIndex)
    {
        int currentIndex = list.IndexOf(element);
        bool missing = currentIndex == -1;

        if (missing)
        {
            throw new ArgumentException(GenerateMoveFailedString(element));
        }

        list.RemoveAt(currentIndex);
        list.Insert(newIndex, element);
    }

    static string GenerateMoveFailedString<T>(T item)
    {
        return $"You are trying to move the following object when we do not have it in this collection: {typeof(T)} \"{item}\"";
    }

    static string GenerateSwapFailedString<T>(T item1, T item2)
    {
        return $"You are trying to swap the following objects when we do not have either in this collection: {typeof(T)} \"{item1}\" & \"{item2}\"";
    }

    /// <summary>
    /// Ever hate writing ".Count - 1"?
    /// </summary>
    public static int LastIndex<T>(this List<T> list)
    {
        return list.Count - 1;
    }

    /// <summary>
    /// Ever hate writing ".Length - 1"?
    /// </summary>
    public static int LastIndex(this Array array)
    {
        return array.Length - 1;
    }

    /// <summary>
    /// Moves provided element of this array to the end of this list (index = Count - 1)
    /// </summary>
    /// <param name="element">An item currently in this list</param>
    public static void MoveItemToEnd<T>(this Array array, T element)
    {
        array.MoveItemTo(element, array.Length - 1);
    }

    /// <summary>
    /// Moves provided element of this array to the start of this list (index = 0)
    /// </summary>
    /// <param name="element">An item currently in this list</param>
    public static void MoveItemToFront<T>(this Array array, T element)
    {
        array.MoveItemTo(element, 0);
    }

    /// <summary>
    /// Moves provided element of this array to the provided index
    /// </summary>
    /// <param name="element">An item currently in this list</param>
    public static void MoveItemTo<T>(this Array array, T element, int newIndex)
    {
        int currentIndex = Array.IndexOf(array, element);
        bool missing = currentIndex == -1;

        if (missing)
        {
            throw new ArgumentException(GenerateMoveFailedString(element));
        }

        T other = (T)array.GetValue(newIndex);
        array.SetValue(element, newIndex);
        array.SetValue(other, currentIndex);
    }

    /// <summary>
    /// Make two elements of this array swap places
    /// </summary>
    /// <param name="itemToSwap1">an item currently in this list</param>
    /// <param name="itemToSwap2">an item currently in this list</param>
    public static void Swap<T>(this Array array, T itemToSwap1, T itemToSwap2)
    {
        int firstIndex = Array.IndexOf(array, itemToSwap1);
        int secondIndex = Array.IndexOf(array,itemToSwap2);

        bool missingFirst = firstIndex == -1;
        bool missingSecond = secondIndex == -1;
        bool missingBoth = missingFirst && missingSecond;

        if (missingBoth)
        {
            throw new ArgumentException(GenerateSwapFailedString(itemToSwap1, itemToSwap2));
        }

        if (missingFirst)
        {
            throw new ArgumentException(GenerateMoveFailedString(itemToSwap1));
        }

        if (missingSecond)
        {
            throw new ArgumentException(GenerateMoveFailedString(itemToSwap2));
        }

        array.SetValue(itemToSwap2, firstIndex);
        array.SetValue(itemToSwap1, secondIndex);
    }
}
