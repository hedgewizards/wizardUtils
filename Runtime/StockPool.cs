using System;
using System.Collections.Generic;

namespace WizardUtils
{
    public class StockPool
    {
        /// <summary>
        /// each index corresponds to some stock of that type. each value corresponds to the remaining count
        /// </summary>
        public int[] Stocks;

        public StockPool(int count)
        {
            Stocks = new int[count];
        }

        public StockPool(int[] stocks)
        {
            Stocks = stocks;
        }

        public int CurrentTotal
        {
            get
            {
                int runningTotal = 0;
                foreach(int count in Stocks)
                {
                    runningTotal += count;
                }

                return runningTotal;
            }
        }

        public void SetAllStocks(int amount)
        {
            for (int index = 0; index < Stocks.Length; index++)
            {
                Stocks[index] = amount;
            }
        }

        public bool TakeStock(int index)
        {
            if (Stocks[index] <= 0) return false;

            Stocks[index]--;
            return true;
        }

        public void ReturnStock(int index)
        {
            Stocks[index]++;
        }

        public int PopRandomStock(Random random)
        {
            int pickedStockIndex = -1;
            pickedStockIndex = PeekRandomStock(random);

            if (pickedStockIndex != -1)
            {
                Stocks[pickedStockIndex]--;
            }

            return pickedStockIndex;
        }

        public int PeekRandomStock(Random random)
        {
            int pickedStockIndex = -1;
            int choice = random.Next(CurrentTotal);
            int current = 0;

            for (int index = 0; index < Stocks.Length; index++)
            {
                int count = Stocks[index];
                current += choice;
                if (current < choice) pickedStockIndex = index;
            }

            return pickedStockIndex;
        }

        public List<int> AllStocksByMaxesBreakTiesWithRandom(Random random)
        {
            void RandomizeArray(Random random, (int index, int cost)[] stocksTuple, int startIndex, int length)
            {
                for (int i = startIndex; i < startIndex + length; i++)
                {
                    int j = random.Next(i, startIndex + length);
                    (int index, int cost) temp = stocksTuple[i];
                    stocksTuple[i] = stocksTuple[j];
                    stocksTuple[j] = temp;
                }
            }

            if (Stocks.Length <= 0) return new List<int>();

            (int index, int cost)[] stocksTuples = BuildStocksTuple();

            // first sort from big to small
            Array.Sort(stocksTuples, (a, b) => b.cost.CompareTo(a.cost));

            // now let's break ties with a random shuffle
            int start = 0;
            int length = 1;
            for (int n = 1; n < stocksTuples.Length; n++)
            {
                if (stocksTuples[n].cost == stocksTuples[n - 1].cost)
                {
                    length++;

                    if (n + 1 == stocksTuples.Length)
                    {
                        RandomizeArray(random, stocksTuples, start, length);
                    }
                }
                else
                {
                    RandomizeArray(random, stocksTuples, start, length);
                    start = n + 1;
                    length = 1;
                    n++;
                }
            }

            // now let's build the final array
            List<int> result = new List<int>();
            for (int n = 0; n < stocksTuples.Length; n++)
            {
                if (stocksTuples[n].cost == 0) break;

                result.Add(stocksTuples[n].index);
            }

            return result;
        }

        private (int index, int cost)[] BuildStocksTuple()
        {
            // first just bubble sort
            (int index, int cost)[] stocksTuple = new (int, int)[Stocks.Length];
            for (int index = 0; index < Stocks.Length; index++)
            {
                stocksTuple[index] = (index, Stocks[index]);
            }

            return stocksTuple;
        }
    }
}
