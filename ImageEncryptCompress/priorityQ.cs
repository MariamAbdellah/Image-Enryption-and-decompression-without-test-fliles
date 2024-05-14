using System;
using System.Collections.Generic;


// Priority Queue implementation
public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> data;

    public int Count => data.Count;

    public PriorityQueue()
    {
        data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        int childIndex = data.Count - 1;

        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
                break;

            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        int lastIndex = data.Count - 1;
        T firstItem = data[0];
        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);

        int parentIndex = 0;
        while (true)
        {
            int childIndex = parentIndex * 2 + 1;
            if (childIndex >= lastIndex)
                break;

            int rightChildIndex = childIndex + 1;
            if (rightChildIndex < lastIndex && data[rightChildIndex].CompareTo(data[childIndex]) < 0)
                childIndex = rightChildIndex;

            if (data[parentIndex].CompareTo(data[childIndex]) <= 0)
                break;

            Swap(parentIndex, childIndex);
            parentIndex = childIndex;
        }

        return firstItem;
    }

    private void Swap(int i, int j)
    {
        T temp = data[i];
        data[i] = data[j];
        data[j] = temp;
    }
}



//==========>> geeks for geeks
// Structure for the elements in the
// priority queue
//public class item
//{
//	public byte value;
//	public int priority;
//};


//public class priorityQ
//{

//	// Store the element of a priority queue
//	static List<item> pr = new List<item>();


//	// Pointer to the last index
//	static int size = -1;
//	// Function to insert a new element
//	// into priority queue
//	public void enqueue(byte value, int priority)
//	{
//		// Increase the size
//		size++;

//		// Insert the element
//		pr[size] = new item();
//		pr[size].value = value;
//		pr[size].priority = priority;
//	}

//	// Function to check the top element
//	public static int peek()
//	{
//		int highestPriority = int.MaxValue;//int.MinValue;
//		int ind = -1;

//		// Check for the element with
//		// highest priority
//		for (int i = 0; i <= size; i++)
//		{

//			// If priority is same choose
//			// the element with the
//			// highest value
//			if (highestPriority == pr[i].priority && ind > -1
//				&& pr[ind].value < pr[i].value)//////////
//			{
//				highestPriority = pr[i].priority;
//				ind = i;
//			}
//			else if (highestPriority > pr[i].priority)
//			{
//				highestPriority = pr[i].priority;
//				ind = i;
//			}
//		}

//		// Return position of the element
//		return ind;
//	}

//	// Function to remove the element with
//	// the highest priority
//	public byte dequeue()
//	{
//		// Find the position of the element
//		// with highest priority
//		int ind = peek();

//		byte val = pr[ind].value;
//		// Shift the element one index before
//		// from the position of the element
//		// with highest priority is found
//		for (int i = ind; i < size; i++)
//		{
//			pr[i] = pr[i + 1];
//		}

//		// Decrease the size of the
//		// priority queue by one
//		size--;

//		return val;
//	}

//	public int count()
//    {
//		return size;
//    }
//}

