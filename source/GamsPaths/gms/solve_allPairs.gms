$set GDXINPUT "network250"
set i,e(i,i),pre(i,i,i);
alias(i,j,o);
parameter c(i,j),b(o,i);
positive variable x(o,i,j);
free variable Z objective;
equations objF, balance;
objF.. Z=E= sum( (o,E), c(E) * x(o,E) ) ;
balance(o,i).. sum(E(i,j), x(o,i,j) ) - sum(E(j,i), x(o,j,i) ) =E= b(o,i);
model APSPP /objF,balance/;
*//GET DATA
$GDXIN %GDXINPUT%
$loaddc i c
$GDXIN
e(i,j) = (c(i,j) > 0);
*//SETUP PROBLEM
b(o,i)=-1;
b(i,i)=card(i)-1;
*//SOLVE+EXPORT
APSPP.optfile=1;
$onecho > cplex.opt
threads 0
parallelmode 0
lpmethod 3
$offecho
solve APSPP minimizing Z using lp;
parameter lpsolv_stats;
lpsolv_stats('Calculation (sec.)') = APSPP.resusd;
lpsolv_stats('Data reading (sec.)') = APSPP.resgen;
lpsolv_stats('Number of Verticies') = card(i);
lpsolv_stats('Number of Edges') = card(E);
lpsolv_stats('Edges per Vertex') = card(E)/card(i);
Parameter totalDist(o);
*totalDist(o)=sum((i,j), c(i,j) * x.l(o,i,j));
pre(o,i,j)=x.l(o,j,i);

parameter spp(i,j);
spp(i,j)=0;
singleton set prdc(i),crnt(i);
LOOP(o,
  LOOP(j$(not sameas(o,j)),
    prdc(i)=no;
    crnt(j)=yes;
    while(not sameas(prdc,o),
      loop(i$(pre(o,crnt,i)), prdc(i)=yes; );
      spp(o,j) = spp(o,j) + c(prdc,crnt);
      crnt(prdc)=yes;
    );
  );
);

execute_unload '%GDXINPUT%_solver.gdx' lpsolv_stats pre totalDist x spp;


