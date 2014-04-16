This project was an attempt to implement [Zheng et al's 2011 paper](http://e-lc.org/tmp/Xiaoyu__Zheng_2011_12_05_14_35_11.pdf) on adapting [Fortune's algorithm](http://en.wikipedia.org/wiki/Fortune%27s_algorithm) to the sphere. The impetus was that one of my [shallow fluid model prototypes](https://github.com/andyljones/shallow-fluid-model-prototype-2) was to be built around what's known as a [multigrid differential equation solver](http://en.wikipedia.org/wiki/Multigrid_method) which in the general case needs a way to compute Voronoi tesselations. While attempting to debug the algorithm, I also built an interactive frontend for it in Unity:

<p align="center">
<img src="http://i.minus.com/iqKmBV07ZQeLH.png">
</p>

Eventually I abandoned this project after realising that developing a multigrid solver, implementing Fortune's algorithm and designing a [speciality datastructure for its sole use](https://github.com/andyljones/cyclical-deterministic-skiplist) just for a toy project was a bit crazy. I changed the parametrization I was using for my fluid model to a much more intuitive and more-easily-debuggable one, and that was what let me [actually finish it](https://github.com/andyljones/shallow-fluid-model).

As an aside, what stalled this project was floating point errors in the [skiplist implementation](https://github.com/andyljones/cyclical-deterministic-skiplist) that underlaid the sweepline. After realising the importance of interfaces I swapped it out for a simple array implementation, which while slow, *actually worked*. When I find the time to go back and rewrite the skiplist implementation, I intend to come back and fix this up too.
