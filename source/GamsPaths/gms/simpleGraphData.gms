
set i Nodes /i1*i10/;
alias(i,j);
parameter c(i,j);
c(i,j) = uniformint(10,100)$(uniform(0,1)>0.6);

execute_unload "network10.gdx" i c;
