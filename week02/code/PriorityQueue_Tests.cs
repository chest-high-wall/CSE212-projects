using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriorityQueueTests
{
    // Scenario: Enqueue 3 items with different priorities.
    // Expected: Dequeue returns the highest priority value ("B").
    // Defect(s) Found:
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

    // Scenario: Two items share the highest priority; FIFO among ties.
    // Expected: "A" dequeues before "C" because A was enqueued first.
    // Defect(s) Found:
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

    // Scenario: Enqueue low then high then mid priorities.
    // Expected: Highest priority wins regardless of enqueue order.
    // Defect(s) Found:
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

    // Scenario: Dequeue on empty queue.
    // Expected: InvalidOperationException with exact message "The queue is empty."
    // Defect(s) Found:
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
