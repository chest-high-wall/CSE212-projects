using System;
using System.Collections;
using System.Collections.Generic;

public static class Recursion
{
    // Problem 1 – Recursive Squares
    // Using recursion, find the total of 1² + 2² + 3² + ... + n².
    // Base case: if n <= 0 return 0. Each call adds n*n then calls itself with n-1.
    public static int SumSquaresRecursive(int n)
    {
        if (n <= 0)
            return 0;
        return n * n + SumSquaresRecursive(n - 1);
    }

    // Problem 2 – Permutations Choose
    // Using recursion, build all possible strings of a given size
    // from a set of letters. Example: A,B,C with size 2 → AB, AC, BA, BC, CA, CB.
    public static void PermutationsChoose(List<string> results, string letters, int size, string word = "")
    {
        // Stop once we reach the target size
        if (word.Length == size)
        {
            results.Add(word);
            return;
        }

        // Try each character, remove it, then keep going
        for (int i = 0; i < letters.Length; i++)
        {
            char c = letters[i];
            string remaining = letters.Remove(i, 1);
            PermutationsChoose(results, remaining, size, word + c);
        }
    }

    // Problem 3 – Climbing Stairs
    // Count how many different ways a person can climb s stairs
    // when they can take 1, 2, or 3 steps at a time.
    // Uses memoization to remember results for speed.
    public static decimal CountWaysToClimb(int s, Dictionary<int, decimal>? remember = null)
    {
        remember ??= new Dictionary<int, decimal>();

        if (s < 0) return 0;
        if (s == 0) return 1;

        if (remember.ContainsKey(s))
            return remember[s];

        decimal total = CountWaysToClimb(s - 1, remember)
                      + CountWaysToClimb(s - 2, remember)
                      + CountWaysToClimb(s - 3, remember);

        remember[s] = total;
        return total;
    }

    // Problem 4 – Wildcard Binary
    // Takes a string with * wildcards (like 10*1) and produces all possible binary combinations.
    public static void WildcardBinary(string pattern, List<string> results)
    {
        int index = pattern.IndexOf('*');

        // no wildcard left, add pattern to results
        if (index == -1)
        {
            results.Add(pattern);
            return;
        }

        // Replace the * with both 0 and 1, then recurse
        string zeroVersion = pattern[..index] + '0' + pattern[(index + 1)..];
        string oneVersion  = pattern[..index] + '1' + pattern[(index + 1)..];

        WildcardBinary(zeroVersion, results);
        WildcardBinary(oneVersion, results);
    }

    // Problem 5 – Maze Solver
    // Uses recursion to explore all possible paths in a maze from (0,0) to the end point marked with a 2.
    public static void SolveMaze(
        List<string> results,
        Maze maze,
        int x = 0,
        int y = 0,
        List<(int, int)>? currPath = null)
    {
        currPath ??= new List<(int, int)>();
        currPath.Add((x, y));

        // If we reached the end, store this path
        if (maze.IsEnd(x, y))
        {
            results.Add(currPath.AsString());
            return;
        }

        // Directions: right, left, down, up
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            // NOTE: Maze.IsValidMove expects (currPath, x, y)
            if (maze.IsValidMove(currPath, nx, ny))
            {
                var newPath = new List<(int, int)>(currPath);
                SolveMaze(results, maze, nx, ny, newPath);
            }
        }
    }
}
