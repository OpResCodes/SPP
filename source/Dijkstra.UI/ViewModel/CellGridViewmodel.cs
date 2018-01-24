using Dijkstra.UI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShortestPaths.Dijkstra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.UI.ViewModel
{
    public class CellGridViewmodel : ViewModelBase
    {
        public CellGridViewmodel()
        {
            Cells = new ObservableCollection<CellViewmodel>();
        }

        //Properties
        private int _NumberOfRows = 10;
        public int NumberOfRows
        {
            get { return _NumberOfRows; }
            set
            {
                ShowField = false;
                Set(() => NumberOfRows, ref _NumberOfRows, value);
                RaisePropertyChanged(nameof(GridHeight));
                GenerateCells.RaiseCanExecuteChanged();
                NumberOfColumns = value;
            }
        }

        public double GridHeight { get { return _NumberOfRows * 12.0; } }

        public double GridWidth { get { return _NumberOfColumns * 12.0; } }

        private int _NumberOfColumns = 10;
        public int NumberOfColumns
        {
            get { return _NumberOfColumns; }
            set
            {
                ShowField = false;
                Set(() => NumberOfColumns, ref _NumberOfColumns, value);
                RaisePropertyChanged(nameof(GridWidth));
                GenerateCells.RaiseCanExecuteChanged();
            }
        }

        private bool _ShowField = false;
        public bool ShowField
        {
            get { return _ShowField; }
            set
            {
                Set(() => ShowField, ref _ShowField, value);
                CalculateCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<CellViewmodel> Cells { get; set; }

        //Methods
        private async Task CreateCells()
        {
            Cells.Clear();
            _SourceAndSink.Clear();
            var x = new CellViewmodel[_NumberOfRows, _NumberOfColumns];
            var cells = new List<CellViewmodel>();
            await Task.Run(() =>
            {
                for (int c = 0; c < _NumberOfColumns; c++)
                {
                    for (int r = 0; r < _NumberOfRows; r++)
                    {
                        x[r, c] = new CellViewmodel();
                        if (c > 0)
                        {
                            //add left neighbour cell
                            x[r, c].AddNeighbour(x[r, c - 1]);
                        }
                        if (r > 0)
                        {
                            //add upper neighbour
                            x[r, c].AddNeighbour(x[r - 1, c]);
                        }
                        cells.Add(x[r, c]);
                    }
                }
            });

            Cells = new ObservableCollection<CellViewmodel>(cells);
            RaisePropertyChanged(nameof(Cells));
            ShowField = true;
        }

        private void UpdateResultPath(ShortestPath path)
        {
            foreach (var node in path.OrderedNodes)
            {
                var cell = Cells.First(c => c.Id == node.Id);
                if (cell.CellState != CellState.IsSelected)
                    cell.CellState = CellState.IsPath;
            }
        }

        private void CreateRandomObstacles(int scale)
        {
            int max = Cells.Count;
            Random r = new Random();
            int imax = (scale > 0) ? 6 : 3;
            for (int i = 0; i < 3; i++)
            {
                var row = r.Next(1, _NumberOfRows);
                var col = r.Next(1, _NumberOfRows);
                var c = (row - 1) * _NumberOfColumns + col;
                var depth = (scale>0) ? r.Next(6, 20) : r.Next(3,10);
                while (depth > 0)
                {
                    var cell = Cells[c];
                    if (cell.CellState != CellState.IsSelected)
                        cell.CellState = CellState.IsInactive;
                    var nextCell = r.Next(cell.Neighbours.Count);
                    c = Cells.IndexOf(cell.Neighbours[nextCell]);
                    depth--;
                }
            }
        }

        //Commands
        public RelayCommand CalculateCommand
        {
            get
            {
                if (_calculateCommand == null)
                {
                    _calculateCommand = new RelayCommand(() =>
                    {
                        if (_SourceAndSink.Count != 2)
                            return;

                        GraphGen gg = new GraphGen();
                        var graph = gg.GenerateGraph(this);
                        var calc = new Calculator(graph);
                        var path = calc.CalculateShortestPath(_SourceAndSink[0].Id, _SourceAndSink[1].Id, false);
                        UpdateResultPath(path);
                        Debug.WriteLine(path.TotalWeight);
                    },
                    () => _SourceAndSink.Count == 2 && ShowField);
                }
                return _calculateCommand;
            }
        }

        public RelayCommand<CellViewmodel> CellSelectedCommand
        {
            get
            {
                if (_CellSelectedCommand == null)
                {
                    _CellSelectedCommand = new RelayCommand<CellViewmodel>((c) =>
                    {
                        if (c.CellState == CellState.IsSelected)
                        {
                            _SourceAndSink.Remove(c);
                            c.CellState = CellState.IsActive;
                        }
                        else if (c.CellState == CellState.IsInactive)
                        {
                            c.CellState = CellState.IsActive;
                        }
                        else
                        {
                            c.CellState = CellState.IsInactive;
                        }
                        CalculateCommand.RaiseCanExecuteChanged();
                    });
                }
                return _CellSelectedCommand;
            }
        }

        public RelayCommand<CellViewmodel> CellActivatedCommand
        {
            get
            {
                if (_CellActivatedCommand == null)
                {
                    _CellActivatedCommand = new RelayCommand<CellViewmodel>((c) =>
                    {
                        if (_SourceAndSink.Count == 2)
                        {
                            _SourceAndSink[0].CellState = CellState.IsActive;
                            _SourceAndSink.RemoveAt(0);
                        }
                        c.CellState = CellState.IsSelected;
                        _SourceAndSink.Add(c);
                        CalculateCommand.RaiseCanExecuteChanged();
                    });
                }
                return _CellActivatedCommand;
            }
        }

        public RelayCommand<string> CreateObstaclesCommand
        {
            get
            {
                if (_createObstaclesCommand == null)
                {
                    _createObstaclesCommand = new RelayCommand<string>(i => CreateRandomObstacles(int.Parse(i)));
                }
                return _createObstaclesCommand;
            }
        }

        public RelayCommand GenerateCells
        {
            get
            {
                if (_GenerateCells == null)
                {
                    _GenerateCells = new RelayCommand(async () => { await CreateCells(); },
                        () => NumberOfColumns > 0 && NumberOfRows > 0);
                }

                return _GenerateCells;
            }
        }

        private RelayCommand _GenerateCells = null;
        private RelayCommand<CellViewmodel> _CellSelectedCommand;
        private RelayCommand<CellViewmodel> _CellActivatedCommand;
        private RelayCommand _calculateCommand;

        //Other fields
        internal List<CellViewmodel> _SourceAndSink = new List<CellViewmodel>();
        private RelayCommand<string> _createObstaclesCommand;
    }
}
