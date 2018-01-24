using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.UI.ViewModel
{
    public class CellViewmodel : ObservableObject
    {
        public CellViewmodel(int id)
        {
            Id = id;
            Neighbours = new ObservableCollection<CellViewmodel>();
        }

        public CellViewmodel()
        {
            _counter++;
            Id = _counter;
            Neighbours = new ObservableCollection<CellViewmodel>();
        }

        public int Id { get; set; }


        private CellState _CellState = CellState.IsActive;
        public CellState CellState
        {
            get { return _CellState; }
            set { Set(() => CellState, ref _CellState, value); }
        }


        public void AddNeighbour(CellViewmodel cellViewmodel)
        {
            if (!Neighbours.Contains(cellViewmodel))
            {
                Neighbours.Add(cellViewmodel);
            }
            if (!cellViewmodel.Neighbours.Contains(this))
            {
                cellViewmodel.Neighbours.Add(this);
            }
        }

        public ObservableCollection<CellViewmodel> Neighbours { get; set; }

        public IEnumerable<CellViewmodel> GetActiveNeighbours()
        {
            if (Neighbours == null || Neighbours.Count == 0)
                return Enumerable.Empty<CellViewmodel>();

            return Neighbours.Where(n => n.CellState == CellState.IsActive || n.CellState == CellState.IsSelected);
        }

        private static int _counter = 0;

    }

    public enum CellState
    {
        IsActive,
        IsInactive,
        IsSelected,
        IsPath
    }
}
