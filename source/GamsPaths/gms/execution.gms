$OffListing
*SOLVE SHORTEST PATH PROBLEMS ALGORITHMIC
$Set ALGPATH "spp\GamsPaths.exe"
$Set GDXINPUT "network250.gdx"

$Ontext
DIJKSTRA'S ALGORITHM: ALL-TO-ALL SHORTEST PATHS (Repeat)
----------------------------------------------------------
Note: If graph has edges of weight=0, these edges need to
be set to have a weight of EPS in order for the algorithm to recognize respective
edges (zero-values will not be stored in GDX files)! The algorithm will transform
zero-edges back from EPS to zero.
Parameters for Dijkstra:
  gdx=()            inputfile containing edges and edgeweights
  v=()              1-dim. set of verticies
  w=()              2-dim. set of edge-weights
$Offtext
Execute '%system.FP%%ALGPATH% gdx="%system.FP%%GDXINPUT%" v=I w=c'
