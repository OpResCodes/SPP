using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPaths.Algorithms.Dijkstra
{
    internal sealed class NodeQueue
    {
        #region Constructors

        internal NodeQueue(IEnumerable<Node> initialNodes)
        {
            _baseHeap = new List<Node>();
            foreach (Node n in initialNodes)
            {
                Enqeue(n);
            }
        }

        internal NodeQueue() : this(Enumerable.Empty<Node>()) { }

        #endregion

        #region Properties

        internal bool IsEmpty
        {
            get { return _baseHeap.Count == 0; }
        }

        private int LastItem
        {
            get
            {
                int count = _baseHeap.Count;
                return count > 0 ? count - 1 : 0;
            }
        }

        private int FirstItem
        {
            get { return 0; }
        }

        private Node HeapMinimum
        {
            get
            {
                return _baseHeap[0];
            }
        }

        #endregion

        #region Methods

        internal Node ExtractMinimum()
        {
            if (_baseHeap.Count < 1) throw new Exception("heap underflow");

            var min = HeapMinimum;
            //rebuild heap
            ExchangeElements(FirstItem, LastItem);
            _baseHeap.RemoveAt(LastItem);
            Heapify(FirstItem);
            return min;
        }

        internal void Enqeue(Node node)
        {
            _baseHeap.Add(node);
            SlideUpwards(LastItem);
        }

        internal void UpdateNodePosition(Node node, double updatedWeight)
        {
            if (!IsLess(updatedWeight, node.DistanceFromSource))
                throw new Exception("New Distance must be smaller than distance flag when updating nodes!");

            node.DistanceFromSource = updatedWeight;
            int pos = node.PositionInQueue;
            //restore heap structure (lower priority nodes slide upwards)
            SlideUpwards(pos);
        }

        internal void Clear()
        {
            _baseHeap.Clear();
        }

        private bool IsLess(double w1, double w2)
        {
            return w1 < w2;
        }

        private void ExchangeElements(int pos1, int pos2)
        {
            //switch
            var v = _baseHeap[pos1];
            _baseHeap[pos1] = _baseHeap[pos2];
            _baseHeap[pos2] = v;

            _baseHeap[pos1].PositionInQueue = pos1;
            _baseHeap[pos2].PositionInQueue = pos2;
        }

        private void SlideUpwards(int pos)
        {
            while (pos > 0 && !ParentChildProperty(pos))
            {
                ExchangeElements(pos, GetParent(pos));
                pos = GetParent(pos);
            }
        }

        private bool ParentChildProperty(int pos)
        {
            return IsLess(_baseHeap[GetParent(pos)].DistanceFromSource, _baseHeap[pos].DistanceFromSource);
        }

        private void Heapify(int pos)
        {
            //check if left or right child of Vertex are smaller, 
            //the smallest of both changes position with parent 
            //which now needs to be reexamined with its new children

            if (pos > LastItem || pos < FirstItem) return;
            int smallest;
            int left = GetLeftChild(pos);
            int right = GetRightChild(pos);

            if (left <= LastItem && IsLess(
                _baseHeap[left].DistanceFromSource,
                _baseHeap[pos].DistanceFromSource))
            {
                smallest = left;
            }
            else
            {
                smallest = pos;
            }

            if (right <= LastItem && IsLess(
                _baseHeap[right].DistanceFromSource,
                _baseHeap[smallest].DistanceFromSource))
            {
                smallest = right;
            }

            if (smallest != pos)
            {
                ExchangeElements(pos, smallest);
                Heapify(smallest);
            }

        }

        private int GetParent(int pos)
        {
            if (pos == FirstItem) return 0;
            return (int)(Convert.ToDouble(pos - 1) / 2); //cast simply drops decimals
        }

        private int GetLeftChild(int pos)
        {
            return 2 * pos + 1;
        }

        private int GetRightChild(int pos)
        {
            return 2 * pos + 2;
        }

        #endregion

        #region Fields

        private readonly List<Node> _baseHeap;

        #endregion
    }

}
