using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriorityQueueTests
{
    // Defect(s) Found: None in this case. Highest priority element was removed correctly.

    [TestMethod]
    public void Dequeue_Removes_HighestPriority()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 1);
        pq.Enqueue("B", 5);  // highest
        pq.Enqueue("C", 3);

        var first = (string)pq.Dequeue();
        Assert.AreEqual("B", first, "Should remove value with highest priority (5).");
    }

    // Defect(s) Found: Tie-breaking was incorrect (LIFO/unstable order).
    // The second dequeue returned A again instead of C (Expected C, Actual A).
    // Root cause: selection logic didn’t use stable FIFO when priorities were equal.

    [TestMethod]
    public void Dequeue_Resolves_Ties_Using_FIFO()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 5); // first with priority 5
        pq.Enqueue("B", 3);
        pq.Enqueue("C", 5); // second with priority 5

        var first = (string)pq.Dequeue();
        var second = (string)pq.Dequeue();

        Assert.AreEqual("A", first, "Among equal priorities, earliest enqueued should be removed first.");
        Assert.AreEqual("C", second, "Then the next item with the same priority.");
    }

    // Defect(s) Found: Highest-priority selection was wrong (Expected C, Actual B).
    // Root cause: Dequeue logic was selecting by enqueue order instead of highest priority value.

    [TestMethod]
    public void Dequeue_Ignores_EnqueueOrder_When_PrioritiesDiffer()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 1);
        pq.Enqueue("B", 2);
        pq.Enqueue("C", 10); // should dequeue first

        var first = (string)pq.Dequeue();
        Assert.AreEqual("C", first, "Priority should dominate enqueue order.");
    }

    // Defect(s) Found: None. Exception type and message matched exactly as required.

    [TestMethod]
    public void Dequeue_Empty_Throws_InvalidOperation_With_Message()
    {
        var pq = new PriorityQueue();

        try
        {
            pq.Dequeue();
            Assert.Fail("Expected InvalidOperationException.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("The queue is empty.", ex.Message);
        }
    }
}
